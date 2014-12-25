using System;

namespace Cecil.Decompiler.Languages
{
    public class ColoredConsoleFormatter : IFormatter
    {
        #region Color settings

        private const ConsoleColor TextColor = ConsoleColor.Gray;
        private const ConsoleColor TokenColor = ConsoleColor.Gray;
        private const ConsoleColor CommentColor = ConsoleColor.Green;
        private const ConsoleColor KeywordColor = ConsoleColor.Blue;
        private const ConsoleColor LiteralColor = ConsoleColor.DarkYellow;
        private const ConsoleColor ReferenceColor = ConsoleColor.DarkRed;
        private const ConsoleColor DefinitionColor = ConsoleColor.Cyan;
        private const ConsoleColor IdentifierColor = ConsoleColor.White;
        #endregion


        bool write_indent;
        int indent;

        public void Write(string str)
        {
            Console.ForegroundColor = TextColor;
            WriteIndent();
            Console.Write(str);
            write_indent = false;
        }

        public void WriteLine()
        {
            Console.WriteLine();
            write_indent = true;
            Console.ForegroundColor = TextColor;
        }

        public void WriteSpace()
        {
            Console.ForegroundColor = TextColor;
            Write(" ");
        }

        public void WriteToken(string token)
        {
            Console.ForegroundColor = TokenColor;
            WriteIndent();
            Console.Write(token);
        }

        public void WriteComment(string comment)
        {
            Console.ForegroundColor = CommentColor;
			      WriteIndent();
            Console.Write(comment);
           // WriteLine();
        }
        
        public void WriteKeyword(string keyword)
        {
            Console.ForegroundColor = KeywordColor;
			WriteIndent();
            Console.Write(keyword);
        }

        public void WriteLiteral(string literal)
        {
            Console.ForegroundColor = LiteralColor;
            Console.Write(literal);
        }

        public void WriteDefinition(string value, object definition)
        {
            Console.ForegroundColor = DefinitionColor;
			WriteIndent();
            Console.Write(value);
			write_indent = false;
        }

        public void WriteReference(string value, object reference)
        {
            Console.ForegroundColor = ReferenceColor;
			WriteIndent();
            Console.Write(value);
			write_indent = false;
        }

        public void WriteIdentifier(string value, object identifier)
        {
            Console.ForegroundColor = IdentifierColor;
			WriteIndent();
            Console.Write(value);
			write_indent = false;
        }

        public void Indent()
        {
            indent++;
        }

        public void Outdent()
        {
            indent--;
        }

        void WriteIndent()
        {
            if (!write_indent)
                return;

            for (int i = 0; i < indent; i++)
                Console.Write("\t");
        }

    }
}
