/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
  
//using SolidOpt.Cache;

namespace SolidOpt.Services.Subsystems.Cache.Demo.Cache1
{
  /// <summary>
  /// Description of TestMemIsolatedStorage.
  /// </summary>
  public class TestMemIsolatedStorage1Collision
  {
    private IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForAssembly();
    protected CacheManager<long,ResultClass> cache;
    
    public TestMemIsolatedStorage1Collision(): base()
    {
      isf.CreateDirectory("cache");
      this.cache = new CacheManager<long,ResultClass>(
        new CacheManager<long,ResultClass>.InitDelegate(Init),
        new CacheManager<long,ResultClass>.TouchDelegate(Touch),
        new CacheManager<long,ResultClass>.ValidateDelegate(Validate),
        new CacheManager<long,ResultClass>.CalculateDelegate(Calculate),
        new CacheManager<long,ResultClass>.UpdateDelegate(Update),
        new CacheManager<long,ResultClass>.DeleteDelegate(Delete)
      );
    }
    
    public virtual void Test()
    {
      Console.WriteLine("ISF: {0}", isf.GetDirectoryNames("*"));
      
      Console.WriteLine("Test {0}:", this.GetType().Name);
      
      Console.WriteLine("cache->{0}", cache);
      cache[1] = ClassBuilder.BuildResult(1);
      Thread.Sleep(2000);
      cache[2] = ClassBuilder.BuildResult(2);
      
      cache[3] = ClassBuilder.BuildResult(3);
      cache[0x300000000] = ClassBuilder.BuildResult(30);
      cache[0x300000000] = ClassBuilder.BuildResult(33);
      
      cache[4] = ClassBuilder.BuildResult(4);
      Console.WriteLine("cache->{0}", cache);
      Console.WriteLine("cache[1]->{0}", cache[1]);
      Console.WriteLine("cache->{0}", cache);
      cache.Remove(1);
      Console.WriteLine("cache->{0}", cache);
      Console.WriteLine("cache[1]->{0}", cache[1]);
      Console.WriteLine("cache->{0}", cache);
      Console.WriteLine("cache[2]->{0}", cache[2]);
      Console.WriteLine("cache->{0}", cache);
      
      Console.Write("Serializing state...");
      System.IO.FileStream fs = new FileStream("cache_"+this.GetType().Name+".txt", FileMode.Create);
      BinaryFormatter bf = new BinaryFormatter();
      bf.Serialize(fs, cache.State);
      fs.Close();
      Console.WriteLine("Done.");
      
      Console.Write("Deserializing state...");
      fs = new FileStream("cache_"+this.GetType().Name+".txt", FileMode.Open);
      bf = new BinaryFormatter();
      cache.State = bf.Deserialize(fs);
      fs.Close();
      Console.WriteLine("Done.");
      Console.WriteLine("cache->{0}", cache);
      
      Console.WriteLine("---");
      Console.WriteLine();
    }
    
    public virtual CacheManager<long,ResultClass>.CacheItem Init(long key, ResultClass value)
    {
      return new CacheIsolatedStorageItem(value, isf, key, DateTime.UtcNow.AddSeconds(1));
    }
    
    public virtual CacheManager<long,ResultClass>.CacheItem Touch(long key, CacheManager<long,ResultClass>.CacheItem item)
    {
//      ((CacheIsolatedStorageItem)item).ExpireAt = ((CacheIsolatedStorageItem)item).ExpireAt.AddSeconds(1);
      return item;
    }
    
    public virtual bool Validate(long key, CacheManager<long,ResultClass>.CacheItem item)
    {
      //return ((CacheIsolatedStorageItem)item).ExpireAt > DateTime.UtcNow;
      return true;
    }
    
    public virtual CacheManager<long,ResultClass>.CacheItem Calculate(long key)
    {
      return new CacheIsolatedStorageItem(ClassBuilder.BuildResult(0), isf, key, DateTime.UtcNow.AddSeconds(1)); //key
    }
    
    public virtual void Update(long key,  CacheManager<long,ResultClass>.CacheItem oldItem)
    {
      oldItem.Value.Text += "!";
    }
    
    public virtual void Delete(long key, CacheManager<long,ResultClass>.CacheItem item)
    {
      ((CacheIsolatedStorageItem)item).Remove(key, isf);
    }
    
    [Serializable]
    public class CacheIsolatedStorageItem: CacheManager<long,ResultClass>.MemoryCacheItem
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
              
      public CacheIsolatedStorageItem(ResultClass value, IsolatedStorageFile isf, long key, DateTime expireAt): base(value)
      {
        IsolatedStorageFileStream isfsValue, isfsExpire, isfsKey;
        
        string path = Path.Combine("cache", key.GetHashCode().ToString());
        isf.CreateDirectory(path);
        
        
        long keyCandidate = ResolveKeyCollision(path, isf, key);
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
      
      internal int ResolveKeyCollision(string path, IsolatedStorageFile isf, long key)
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
      
      public void Remove(long key, IsolatedStorageFile isf)
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
