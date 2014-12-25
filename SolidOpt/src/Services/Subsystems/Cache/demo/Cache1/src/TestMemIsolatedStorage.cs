/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;

//using SolidOpt.Cache;

namespace SolidOpt.Services.Subsystems.Cache.Demo.Cache1
{
  
  /// <summary>
  /// Description of TestMemIsolatedStorage.
  /// </summary>
  public class TestMemIsolatedStorage: TestBase
  {
    private IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForAssembly();
    
    public TestMemIsolatedStorage(): base()
    {
      isf.CreateDirectory("cache");
      this.cache = new CacheManager<int,ResultClass>(
        new CacheManager<int,ResultClass>.InitDelegate(Init),
        new CacheManager<int,ResultClass>.TouchDelegate(Touch),
        new CacheManager<int,ResultClass>.ValidateDelegate(Validate),
        new CacheManager<int,ResultClass>.CalculateDelegate(Calculate),
        new CacheManager<int,ResultClass>.UpdateDelegate(Update),
        new CacheManager<int,ResultClass>.DeleteDelegate(Delete)
      );
    }
    
    public override void Test()
    {
      Console.WriteLine("ISF: {0}", isf.GetDirectoryNames("*"));
      base.Test();
    }
    
    public virtual CacheManager<int,ResultClass>.CacheItem Init(int key, ResultClass value)
    {
      return new CacheIsolatedStorageItem(value, isf, key, DateTime.UtcNow.AddSeconds(1));
    }
    
    public virtual CacheManager<int,ResultClass>.CacheItem Touch(int key, CacheManager<int,ResultClass>.CacheItem item)
    {
//      ((CacheIsolatedStorageItem)item).ExpireAt = ((CacheIsolatedStorageItem)item).ExpireAt.AddSeconds(1);
      return item;
    }
    
    public virtual bool Validate(int key, CacheManager<int,ResultClass>.CacheItem item)
    {
      //return ((CacheIsolatedStorageItem)item).ExpireAt > DateTime.UtcNow;
      return true;
    }
    
    public virtual CacheManager<int,ResultClass>.CacheItem Calculate(int key)
    {
      return new CacheIsolatedStorageItem(ClassBuilder.BuildResult(0), isf, key, DateTime.UtcNow.AddSeconds(1)); //key
    }
    
    public virtual void Update(int key, CacheManager<int,ResultClass>.CacheItem oldItem)
    {
      oldItem.Value.Text += "!";
    }
    
    public virtual void Delete(int key, CacheManager<int,ResultClass>.CacheItem item)
    {
      ((CacheIsolatedStorageItem)item).Remove(key, isf);
    }
    
    [Serializable]
    public class CacheIsolatedStorageItem: CacheManager<int,ResultClass>.MemoryCacheItem
    {
      [NonSerialized]
      private DateTime expireAt;      
      public DateTime ExpireAt {
        get { return expireAt; }
        set { expireAt = value; }
      }
      
      public CacheIsolatedStorageItem(): base()
      {
      }
              
      public CacheIsolatedStorageItem(ResultClass value, IsolatedStorageFile isf, int key, DateTime expireAt): base(value)
      {
        IsolatedStorageFileStream isfsValue, isfsExpire, isfsKey;
        
        string path = Path.Combine("cache", key.GetHashCode().ToString());
        isf.CreateDirectory(path);
        
        
        int keyCandidate = ResolveKeyCollision(path, isf, key);
        string suffix = "-" + keyCandidate;
            
        
        
        
        isfsKey = new IsolatedStorageFileStream(Path.Combine(path, "key" + suffix),
                                                FileMode.Create, FileAccess.Write, isf);
        StreamWriter sWriter = new StreamWriter(isfsKey);
        sWriter.Write(key.ToString());
        sWriter.Close();
        
        this.expireAt = expireAt;
        isfsExpire = new IsolatedStorageFileStream(Path.Combine(path, "expireAt" + suffix),
                                                   FileMode.Create, FileAccess.Write, isf);
        sWriter = new StreamWriter(isfsExpire);
        sWriter.Write(expireAt.ToBinary());
        sWriter.Close();
        
        
        isfsValue = new IsolatedStorageFileStream(Path.Combine(path, "value" + suffix), FileMode.Create,
                                                  FileAccess.Write, isf);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(isfsValue, value);
        isfsValue.Close();
      }
      
      internal int ResolveKeyCollision(string path, IsolatedStorageFile isf, int key)
      {
        IsolatedStorageFileStream isfsKey;
        int result = 1;
        string searchKey = key.ToString();
        
        do{
          try{
            isfsKey = new IsolatedStorageFileStream(Path.Combine(path, "key-"+result),
                                                    FileMode.Open, FileAccess.Read, isf);
            StreamReader sReader = new StreamReader(isfsKey);
            if (searchKey == sReader.ReadLine()){
              isfsKey.Close();
              return result;
            }
            else{
              isfsKey.Close();
              result++;
            }
          }
          catch (FileNotFoundException){
            return result;
          }
        }
        while (true);
      }
      
      public void Remove(int key, IsolatedStorageFile isf)
      {
        string path = Path.Combine("cache", key.GetHashCode().ToString());
        int keyCandidate = ResolveKeyCollision(path, isf, key);
        string suffix = "-" + keyCandidate;
        
        foreach (string file in isf.GetFileNames(Path.Combine(path, "*" + suffix))){
          isf.DeleteFile(Path.Combine(path, file));
        }
        
//        isf.DeleteDirectory(path);
      }
      
      public override string ToString()
      {
        return string.Format("{0}:exp={1}", base.ToString(), ExpireAt);
      }
    }
  }
}
