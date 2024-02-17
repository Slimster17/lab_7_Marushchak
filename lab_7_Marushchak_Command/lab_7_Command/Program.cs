using System;
using System.Collections.Generic;

namespace lab_7_Command
{
    class Note // receiver
    {
        public string Text { get; private set; }
        public string Clipborad { get; private set; }

        public void CutText(int startIndex, int length)
        {
            Text = Text.Remove(startIndex, length);
        }

        public void CopyText(int startIndex, int length)
        {
            Clipborad = Text.Substring(startIndex, length);
        }

        public void PasteText(string text)
        {
            Text += text;
        }

        public void SetItalicFont()
        {
            Console.WriteLine("Text has been set to italic ");
        }
        
    }
    
   abstract class Command // common command's class
   {
       protected Note _note;
       protected string _backup;
       protected int _startIndex;
       protected int _length;
       
       public Command(Note note, int startIndex, int length)
       {
           _note = note;
           _startIndex = startIndex;
           _length = length;
       }
        public abstract void Execute();
        public abstract void Undo();
    }
   
   class CutCommand: Command
   {
       public CutCommand(Note note, int startIndex, int length) : base(note, startIndex, length)
       {
           
       }
       public override void Execute()
       {
           _backup = _note.Text.Substring(_startIndex, _length);
           _note.CutText(_startIndex,_length);
       }

       public override void Undo()
       {
           _note.PasteText(_backup);
       }
   }
   
   class CopyCommand : Command
   {
       public CopyCommand(Note note, int startIndex, int length) : base(note, startIndex, length)
       {
           
       }
       
       public override void Execute()
       {
           _note.CopyText(_startIndex, _length);
       }

       public override void Undo()
       {
           throw new NotImplementedException();
       }
       
   }
   
   class PasteCommand : Command
   {
       private string _textToPaste;
       
       public PasteCommand(Note note, string textToPaste, int startIndex) : base(note, startIndex, textToPaste.Length)
       {
           _textToPaste = textToPaste;
       }
       
       public override void Execute()
       {
           _note.PasteText(_textToPaste);
       }

       public override void Undo()
       {
           _note.CutText(_startIndex, _textToPaste.Length);
       }
   }
   
   class ItalicCommand : Command
   {
       public ItalicCommand(Note note, int startIndex, int length) : base(note, 0, 0)
       {
           
       }

       public override void Execute()
       {
           _backup = _note.Text;
           _note.SetItalicFont();
       }

       public override void Undo()
       {
           _note.PasteText(_backup);
       }
   }

   class Invoker
   {
       private Stack<Command> _commands = new Stack<Command>();

       public void StoreAndExecute(Command command)
       {
           command.Execute();
           _commands.Push(command);
       }

       public void UndoLast()
       {
           if (_commands.Count > 0)
           {
               var lastCommand = _commands.Pop();
               lastCommand.Undo();
           }
       }
       
   }


   internal class Program
    {
        public static void Main(string[] args)
        {
            Note note = new Note();
            Invoker invoker = new Invoker();
            
            note.PasteText("Hello, world!");
            Console.WriteLine("Text before cut: " + note.Text);
            Console.WriteLine(new string('*',50));
       
            invoker.StoreAndExecute(new CutCommand(note, 7, 5));
            Console.WriteLine("Text after cut: " + note.Text);
            Console.WriteLine(new string('*',50));
            
            invoker.UndoLast();
            Console.WriteLine("Text after undo: " + note.Text);
            Console.WriteLine(new string('*',50));

            invoker.StoreAndExecute(new CopyCommand(note, 7, 5));
            Console.WriteLine("Clipboard after copy: " + note.Clipborad);
            Console.WriteLine(new string('*',50));

            invoker.StoreAndExecute(new PasteCommand(note, note.Clipborad, note.Text.Length));
            Console.WriteLine("Text after paste: " + note.Text);
            Console.WriteLine(new string('*',50));

            invoker.StoreAndExecute(new ItalicCommand(note, 0, 0));
        }
    }
}