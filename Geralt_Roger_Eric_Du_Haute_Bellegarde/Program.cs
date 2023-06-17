using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Geralt_Roger_Eric_Du_Haute_Bellegarde
{
    public class ProgramClass
    {

        private bool isHalted;

        private int InstructionPointer;
        private int ProgramStartPoint;

        public bool IsHalted() { return isHalted; }

        public int GetInstructionPointer() { return InstructionPointer; }
        public int GetProgramStartPointer() { return ProgramStartPoint; }

        public void IncrementInstructionPointer() { InstructionPointer++; }
        public void JumpToInstruction(int instructionAddress) { InstructionPointer = instructionAddress; }



        public List<object> DataArray;

        public Dictionary<string, object> DataSegment;

        public List<Instruction> CodeInstructions;

        public Stack<int> ProgramStack;
        public Stack<object> DataStack;

        public Dictionary<string, int> Lables;


        public void stopProgram() { isHalted = true; }

        public ProgramClass()
        {
            InstructionPointer = 0;
            ProgramStartPoint = 0;
            isHalted = false;
            DataSegment = new Dictionary<string, object>();
            CodeInstructions = new List<Instruction>();
            Lables = new Dictionary<string, int>();
            ProgramStack = new Stack<int>();
            DataStack = new Stack<object>();
            DataArray = new List<object>();
        }
    }
    public class MyProgram
    {
        private static string[] Lines;


        public static string OutputStream;



        private static void ShowFatalError(string Message, int Line)
        {
            Console.WriteLine(Message + " at :- " + Line);
            OutputStream +=  "\n"+Message + " at :- " + Line;
        }
        public static void Main(string[] args)
        {
            try
            {
                if (args[0] != null)
                {
                    if (args[0].TrimEnd().TrimStart() == "")
                    {
                        Console.WriteLine("no file name provided");
                        OutputStream = "no file name provided";
                    }
                    else
                    {
                        ProgramClass prog = new ProgramClass();

                        GetStringDataFromFile(args[0], false);
                        if (args.Length >= 2)
                        {
                            for (int i = 1; i < args.Length; i++)
                            {
                                if (Instruction.IsNumberInt(args[i]))
                                    prog.DataArray.Add(Convert.ToInt32(args[i]));
                                else if (Instruction.IsNumberDouble(args[i]))
                                    prog.DataArray.Add(Convert.ToDouble(args[i]));
                                else
                                    prog.DataArray.Add(args[i]);
                            }
                        }

                        RemoveCommentsAndEmptyNewLines();
                        CheckIfEveryLineEndsWithSemiColon();
                        ProcessEachLinesForSpacesAndTabs();
                        SetInstructions(ref prog);
                        RunProgram(prog);
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("no file name provided ");
                OutputStream += "no file name provided ";
            }
        }


        public static void RunProgramOnText(string ProgramCode,string[] Arguments)
        {
            try
            {
                if (ProgramCode != null & ProgramCode != "")
                {
                    ProgramClass prog = new ProgramClass();
                    ConvertProgrameToLines(ProgramCode, false);
                    OutputStream =  "your code output here:-    ";

                    if (Arguments.Length > 0)
                    {
                        for (int i = 1; i < Arguments.Length; i++)
                        {
                            if (Instruction.IsNumberInt(Arguments[i]))
                                prog.DataArray.Add(Convert.ToInt32(Arguments[i]));
                            else if (Instruction.IsNumberDouble(Arguments[i]))
                                prog.DataArray.Add(Convert.ToDouble(Arguments[i]));
                            else
                                prog.DataArray.Add(Arguments[i]);
                        }
                    }

                    RemoveCommentsAndEmptyNewLines();
                    CheckIfEveryLineEndsWithSemiColon();
                    ProcessEachLinesForSpacesAndTabs();
                    SetInstructions(ref prog);


                    if (!CheckIfProgramContainsAnyConsoleInput(prog))
                        RunProgram(prog);
                    else
                    {
                        Console.WriteLine(" code had some thing illegal to not run ");
                        OutputStream += " code had some thing illegal to not run ";
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(" error occured due to " + error.Message);
                OutputStream += " error occured due to " + error.Message;
            }
        }



        private static void RunProgram(ProgramClass prog)
        {
            try
            {
                if (prog != null)
                {
                    while (!prog.IsHalted())
                    {
                        if (prog.GetInstructionPointer() > prog.CodeInstructions.Count - 1 || prog.GetInstructionPointer() < 0)
                        {
                            Console.WriteLine("reached out of the code boundary :- " + prog.GetInstructionPointer() + " use halt instruction if necessary");
                            OutputStream += "reached out of the code boundary :- " + prog.GetInstructionPointer() + " use halt instruction if necessary";
                            break;
                        }
                        prog.CodeInstructions[prog.GetInstructionPointer()].ExecuteInstruction();
                        prog.IncrementInstructionPointer();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Program Halted");
                OutputStream += "reached out of the code boundary :- " + prog.GetInstructionPointer() + " use halt instruction if necessary";
            }
        }
        private static void CheckIfEveryLineEndsWithSemiColon()
        {
            List<string> newLines = new List<string>();
            int counter = 0;
            foreach (string s in Lines)
            {
                if (s.TrimEnd().EndsWith(";"))
                {
                    string[] sarr = s.Split(';');
                    List<string> s1 = new List<string>(sarr);
                    s1.RemoveAt(s1.Count - 1);
                    newLines.AddRange(s1);
                }
                else
                {
                    ShowFatalError(s + " expected ';' at the end of line", counter);
                    return;
                }
                counter++;
            }
            Lines = newLines.ToArray();
        }
        private static void GetStringDataFromFile(string path, bool showData = true)
        {
            try
            {
                string FileData = File.ReadAllText(path, System.Text.Encoding.ASCII);
                Lines = FileData.Split('\n');
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(" the file was not found ");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(" there were no data in file");
            }
        }
        private static void ConvertProgrameToLines(string ProgramCode, bool showData = true)
        {
            try
            {
                Lines = ProgramCode.Split('\n');
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(" the file was not found ");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(" there were no data in file");
            }


        }


        private static bool CheckIfProgramContainsAnyConsoleInput(ProgramClass program)
        {
            try
            {
                foreach (Instruction i in program.CodeInstructions)
                {
                    if (i is Input)
                    {
                        Console.WriteLine(" thisMyProgram execution cannot have input instructions");
                        OutputStream += " thisMyProgram execution cannot have input instructions";
                        return true;
                    }
                }
                return false;
            }
            catch (Exception error)
            {
                Console.WriteLine("error occured due to " + error.Message);
                OutputStream += "error occured due to " + error.Message;
                return true;
            }
        }

        private static void RemoveCommentsAndEmptyNewLines()
        {
            try
            {
                List<string> NewLines = new List<string>();
                string[] MidCommentSplitLine;

                foreach (string line in Lines)
                {
                    if (!line.StartsWith("//") && !string.IsNullOrWhiteSpace(line))
                    {
                        MidCommentSplitLine = line.Split("//".ToCharArray());
                        NewLines.Add(MidCommentSplitLine[0]);
                    }
                }
                Lines = NewLines.ToArray();
            }
            catch (Exception error)
            {
                Console.WriteLine("File data is empty");
            }
        }
        private static void ProcessEachLinesForSpacesAndTabs()
        {
            for (int i = 0; i < Lines.Length; i++)
                Lines[i] = ProcessSpacesAndTabs(Lines[i]);
        }
        private static string ProcessSpacesAndTabs(string line)
        {
            bool isFirstSpace = false;
            bool notAddAnySpace = false;
            bool isInString = false;
            string NewLine = "";

            foreach (char c in line)
            {
                if (!isInString && (c == ' ' || c == '\t'))
                {
                    if (isFirstSpace)
                    {
                        NewLine += " ";
                        isFirstSpace = false;
                        notAddAnySpace = true;
                    }
                    else
                        NewLine += "";
                }
                else
                {
                    if (!notAddAnySpace)
                        isFirstSpace = true;

                    if (c == '"')
                        isInString = !isInString;
                    NewLine += c;
                }
            }
            return NewLine;
        }
        private static void SetInstructions(ref ProgramClass prog)
        {
            try
            {
                Instruction.SetRegistry();
                string[] InstructionAndOperand = new string[2];
                Instruction i = null;
                int counter = 0;
                foreach (string Line in Lines)
                {
                    counter++;
                    InstructionAndOperand = Line.Split(" ".ToCharArray(), 2);
                    if (Instruction.InstructionRegistry.ContainsKey(InstructionAndOperand[0]))
                    {
                        i = Instruction.InstructionRegistry[InstructionAndOperand[0]];
                        prog.CodeInstructions.Add(i.GetNewInstruction(prog, Line));
                    }
                    else
                    {
                        ShowFatalError("there is no Instruction such as :- " + InstructionAndOperand[0], counter);
                        prog = null;
                    }
                }

                //ShowInstructions(prog);
            }
            catch (Exception error)
            {
                prog = null;
            }
        }
    }
    public abstract class Instruction
    {
        public ProgramClass programReference;

        public static Dictionary<string, Instruction> InstructionRegistry;

        public static void SetRegistry()
        {
            if (InstructionRegistry == null)
            {
                InstructionRegistry = new Dictionary<string, Instruction>();

                InstructionRegistry.Add("print", new Print());
                InstructionRegistry.Add("add", new Add());
                InstructionRegistry.Add("sub", new Sub());
                InstructionRegistry.Add("mul", new Mul());
                InstructionRegistry.Add("div", new Div());
                InstructionRegistry.Add("mod", new Mod());
                InstructionRegistry.Add("assign", new Assign());
                InstructionRegistry.Add("declare", new Declare());
                InstructionRegistry.Add("halt", new Halt());
                InstructionRegistry.Add("label", new Label());
                InstructionRegistry.Add("jump", new Jump());
                InstructionRegistry.Add("jump_equals", new JumpEquals());
                InstructionRegistry.Add("jump_string_equals", new JumpStringEquals());
                InstructionRegistry.Add("jump_greater_than", new JumpGreaterThan());
                InstructionRegistry.Add("jump_greater_than_or_equals", new JumpGreaterThanEquals());
                InstructionRegistry.Add("jump_less_than", new JumpLessThan());
                InstructionRegistry.Add("jump_less_than_or_equals", new JumpLessThanEquals());
                InstructionRegistry.Add("input", new Input());
                InstructionRegistry.Add("strlen", new StrLen());
                InstructionRegistry.Add("strcat", new StrCat());
                InstructionRegistry.Add("substr", new StrSubString());
                InstructionRegistry.Add("file", new FileIO());
                InstructionRegistry.Add("return", new Return());
                InstructionRegistry.Add("call", new Call());
                InstructionRegistry.Add("get_type", new GetType());
            }
        }
        public abstract void ExecuteInstruction();

        public abstract Instruction GetNewInstruction(ProgramClass Reference, string Line);
        public void ShowError(string message)
        {
            try
            {
                Console.WriteLine(message + " at line:- " + programReference.GetInstructionPointer());
            }
            catch (Exception error)
            {
            }
        }
        public void ShowFatalError(string message)
        {
            try
            {
                ShowError(message);
                programReference.stopProgram();
            }
            catch (Exception error)
            {

            }
        }
        public static bool IsNumberDouble(string number)
        {
            try
            {
                double d = Convert.ToDouble(number);
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }
        public static bool IsString(string val)
        {
            if (val.TrimStart().StartsWith("\"") && val.TrimEnd().EndsWith("\""))
                return true;
            else
                return false;
        }
        public static bool IsNumberInt(string number)
        {
            try
            {
                int d = Convert.ToInt32(number);
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }
        public void SetData(string var, object val)
        {
            try
            {
                if (var.TrimEnd().TrimStart() == "stack")
                    programReference.DataStack.Push(val);
                else if (programReference.DataSegment.ContainsKey(var))
                {
                    int i = 0; double d = 0;
                    if (
                            programReference.DataSegment[var].GetType() == val.GetType() ||
                            (
                                (programReference.DataSegment[var].GetType() == i.GetType() && val.GetType() == d.GetType()) || (programReference.DataSegment[var].GetType() == d.GetType() && val.GetType() == i.GetType())
                            )
                        )
                        programReference.DataSegment[var] = val;

                    else
                        ShowFatalError(" trying to assign data to mismatch datatype variable");
                }
                else if (var.StartsWith("array@"))
                {
                    string[] arrayAndIndex = var.Split("@".ToCharArray(), 2);
                    object ind = GetData(arrayAndIndex[1]);
                    if (IsNumberInt(ind.ToString()))
                    {
                        int index = Convert.ToInt32(arrayAndIndex[1]);
                        int i = 0; double d = 0;
                        if (
                                programReference.DataArray[index].GetType() == val.GetType() ||
                                (
                                    (programReference.DataArray[index].GetType() == i.GetType() && val.GetType() == d.GetType()) || (programReference.DataArray[index].GetType() == d.GetType() && val.GetType() == i.GetType())
                                )
                            )
                            programReference.DataArray[index] = val;
                        else
                            ShowFatalError(" trying to assign data to mismatch datatype variable");
                    }
                    else
                        ShowFatalError(" the index must be an integer");

                }
                else
                    ShowFatalError("variable does not exists at:- " + var);
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }
        public double GetDouble(string number)
        {
            try
            {
                if (number == "stack")
                {
                    try
                    {
                        object data = programReference.DataStack.Pop();
                        return Convert.ToDouble(data);
                    }
                    catch (Exception error)
                    {
                        ShowFatalError(error.Message);
                    }
                }
                else if (IsNumberDouble(number))
                    return Convert.ToDouble(number);
                else if (IsNumberInt(number))
                    return Convert.ToInt32(number);
                else if (programReference.DataSegment.ContainsKey(number))
                    return Convert.ToDouble(programReference.DataSegment[number]);
                else if (number.StartsWith("array@"))
                {
                    string[] arrayAndIndex = number.Split("@".ToCharArray(), 2);
                    object ind = GetData(arrayAndIndex[1]);
                    if (IsNumberInt(ind.ToString()))
                    {
                        int index = Convert.ToInt32(arrayAndIndex[1]);
                        return Convert.ToDouble(programReference.DataArray[index]);
                    }
                    else
                    {
                        ShowFatalError(" the index must be an integer");
                        return -1;
                    }
                }
                else
                    ShowFatalError("variable does not exists or has incorrect format at:- " + number);

                return -1;
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
                return -1;
            }
        }
        public object GetData(string var)
        {
            try
            {
                if (var == "stack")
                {
                    object data = programReference.DataStack.Pop();
                    return data;
                }
                else if (IsNumberDouble(var))
                    return Convert.ToDouble(var);
                else if (IsNumberInt(var))
                    return Convert.ToInt32(var);
                else if (IsString(var))
                    return var.ToString();
                else if (programReference.DataSegment.ContainsKey(var))
                    return programReference.DataSegment[var];
                else if (var.StartsWith("array@"))
                {
                    string[] arrayAndIndex = var.Split("@".ToCharArray(), 2);
                    object ind = GetData(arrayAndIndex[1]);
                    if (IsNumberInt(ind.ToString()))
                    {
                        int index = Convert.ToInt32(arrayAndIndex[1]);
                        return programReference.DataArray[index];
                    }
                    else
                    {
                        ShowFatalError(" the index must be an integer");
                        return -1;
                    }
                }
                else
                {
                    ShowFatalError("variable does not exIsts or has incorrect format at:- " + var);
                    return -1;
                }
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
                return -1;
            }
        }

        public Instruction(ProgramClass reference)
        {
            programReference = reference;
        }
    }
    public class Print : Instruction
    {
        string PrintDataOrVar;

        public Print(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 2)
                    ShowFatalError("print instruction requires only instruction and print string or variable");
                else
                    PrintDataOrVar = InstructionAndData[1];
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                if (IsString(PrintDataOrVar))
                {
                    Console.WriteLine(PrintDataOrVar.ToString().Replace('"', ' '));
                    MyProgram.OutputStream += PrintDataOrVar.ToString().Replace('"', ' ');
                }
                else
                {
                    if (programReference.DataSegment.ContainsKey(PrintDataOrVar))
                    {
                        object stringData = programReference.DataSegment[PrintDataOrVar];
                        if (stringData != null)
                        {
                            if (IsString(stringData.ToString()))
                            {
                                Console.WriteLine(stringData.ToString().Replace('"', ' '));
                               MyProgram.OutputStream += stringData.ToString().Replace('"', ' ');
                            }
                            else
                            {
                                Console.WriteLine(stringData.ToString());
                               MyProgram.OutputStream += stringData.ToString();
                            }
                        }
                        else
                            ShowError("no data in the variable");
                    }
                    else if (PrintDataOrVar.TrimStart().TrimEnd() == "stack")
                    {
                        object o = programReference.DataStack.Pop();
                        if (IsString(o.ToString()))
                        {
                            Console.WriteLine(o.ToString().Replace('"', ' '));
                           MyProgram.OutputStream += o.ToString().Replace('"', ' ');
                        }
                        else
                        {
                            Console.WriteLine(o.ToString());
                           MyProgram.OutputStream += o.ToString();
                        }
                    }
                    else if (PrintDataOrVar.TrimStart().StartsWith("array@"))
                    {
                        object o = GetData(PrintDataOrVar);
                        if (IsString(o.ToString()))
                        {
                            Console.WriteLine(o.ToString().Replace('"', ' '));
                           MyProgram.OutputStream += o.ToString().Replace('"', ' ');
                        }
                        else
                        {
                            Console.WriteLine(o.ToString());
                           MyProgram.OutputStream += o.ToString();
                        }
                    }

                    else
                        ShowFatalError("no such Variable as \"" + PrintDataOrVar + "\"");
                }
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Print(Reference, Line);
        }
    }
    public class Add : Instruction
    {
        string answer, number1, number2;

        public Add(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                string[] values = InstructionAndData[1].Split(',');

                if (InstructionAndData.Length != 2)
                    ShowFatalError("add instruction requires only instruction and variables or data");
                else
                {
                    if (values.Length != 3)
                        ShowFatalError("add instruction requires three values ans,number 1,number2");
                    else
                    {
                        answer = values[0];
                        number1 = values[1];
                        number2 = values[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                double a, n1, n2;
                n1 = GetDouble(number1);
                n2 = GetDouble(number2);
                a = n1 + n2;
                SetData(answer, a);
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Add(Reference, Line);
        }
    }
    public class Sub : Instruction
    {
        string answer, number1, number2;

        public Sub(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                string[] values = InstructionAndData[1].Split(',');

                if (InstructionAndData.Length != 2)
                    ShowFatalError("sub instruction requires only instruction and variables or data");
                else
                {
                    if (values.Length != 3)
                        ShowFatalError("sub instruction requires three values ans,number 1,number2");
                    else
                    {
                        answer = values[0];
                        number1 = values[1];
                        number2 = values[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                double a, n1, n2;
                n1 = GetDouble(number1);
                n2 = GetDouble(number2);
                a = n1 - n2;
                SetData(answer, a);
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Sub(Reference, Line);
        }
    }
    public class Mul : Instruction
    {
        string answer, number1, number2;

        public Mul(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                string[] values = InstructionAndData[1].Split(',');

                if (InstructionAndData.Length != 2)
                    ShowFatalError("mul instruction requires only instruction and variables or data");
                else
                {
                    if (values.Length != 3)
                        ShowFatalError("mul instruction requires three values ans,number 1,number2");
                    else
                    {
                        answer = values[0];
                        number1 = values[1];
                        number2 = values[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                double a, n1, n2;
                n1 = GetDouble(number1);
                n2 = GetDouble(number2);
                a = n1 * n2;
                SetData(answer, a);
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Mul(Reference, Line);
        }
    }
    public class Div : Instruction
    {
        string answer, number1, number2;

        public Div(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                string[] values = InstructionAndData[1].Split(',');

                if (InstructionAndData.Length != 2)
                    ShowFatalError("div instruction requires only instruction and variables or data");
                else
                {
                    if (values.Length != 3)
                        ShowFatalError("div instruction requires three values ans,number 1,number2");
                    else
                    {
                        answer = values[0];
                        number1 = values[1];
                        number2 = values[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                double a, n1, n2;
                n1 = GetDouble(number1);
                n2 = GetDouble(number2);
                a = n1 / n2;
                SetData(answer, a);
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Div(Reference, Line);
        }
    }
    public class Mod : Instruction
    {
        string answer, number1, number2;

        public Mod(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                string[] values = InstructionAndData[1].Split(',');

                if (InstructionAndData.Length != 2)
                    ShowFatalError("mod instruction requires only instruction and variables or data");
                else
                {
                    if (values.Length != 3)
                        ShowFatalError("mod instruction requires three values ans,number 1,number2");
                    else
                    {
                        answer = values[0];
                        number1 = values[1];
                        number2 = values[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                double a, n1, n2;
                n1 = GetDouble(number1);
                n2 = GetDouble(number2);
                a = n1 % n2;
                SetData(answer, a);
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Mod(Reference, Line);
        }
    }
    public class Assign : Instruction
    {
        string Var1, Var2;

        public Assign(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                string[] values = InstructionAndData[1].Split(',');

                if (InstructionAndData.Length != 2)
                    ShowFatalError("Assign instruction requires only instruction and variables or data");
                else
                {
                    if (values.Length != 2)
                        ShowFatalError("Assign instruction requires 2 values var1,var2");
                    else
                    {
                        Var1 = values[0];
                        Var2 = values[1];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                SetData(Var1, GetData(Var2));
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Assign(Reference, Line);
        }
    }
    public class Declare : Instruction
    {
        string type, name, val;
        public Declare(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                string[] values = InstructionAndData[1].Split(',');

                if (InstructionAndData.Length != 2)
                    ShowFatalError("Declare instruction requires only instruction and variables or data");
                else
                {
                    if (values.Length != 3)
                        ShowFatalError("Declare instruction requires three values type,name and value");
                    else
                    {
                        type = values[0];
                        name = values[1];
                        val = values[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                if (!programReference.DataSegment.ContainsKey(name))
                {
                    if (type == "string")
                    {
                        string s;
                        if (IsString(val))
                            s = val;
                        else
                            s = GetData(val).ToString();


                        if (name != "array")
                            programReference.DataSegment.Add(name, s);
                        else
                        {
                            programReference.DataArray.Add(s);
                            programReference.DataStack.Push(programReference.DataArray.Count);
                        }
                    }
                    else if (type == "int")
                    {
                        int i;
                        if (IsNumberInt(val))
                            i = Convert.ToInt32(val);
                        else
                            i = (int)GetDouble(val);


                        if (name != "array")
                            programReference.DataSegment.Add(name, i);
                        else
                        {
                            programReference.DataArray.Add(i);
                            programReference.DataStack.Push(programReference.DataArray.Count - 1);
                        }
                    }
                    else if (type == "double")
                    {
                        double d;
                        if (IsNumberInt(val))
                            d = Convert.ToInt32(val);
                        else
                            d = GetDouble(val);

                        if (name != "array")
                            programReference.DataSegment.Add(name, d);
                        else
                        {
                            programReference.DataArray.Add(d);
                            programReference.DataStack.Push(programReference.DataArray.Count - 1);
                        }
                    }
                    else
                        ShowFatalError("unknown data type");
                }
                else
                    ShowFatalError("variable already exists ");
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Declare(Reference, Line);
        }
    }
    public class Halt : Instruction
    {
        public Halt(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 1)
                    ShowFatalError("code_end instruction requires only instruction");
            }
        }

        public override void ExecuteInstruction()
        {
            programReference.stopProgram();
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Halt(Reference, Line);
        }
    }
    public class Label : Instruction
    {
        string LabelName;
        public Label(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length == 2)
                {
                    LabelName = InstructionAndData[1];
                    if (programReference.Lables.ContainsKey(LabelName))
                        ShowFatalError("that lable already exists");
                    else
                    {
                        int LineNumber = programReference.CodeInstructions.Count;
                        programReference.Lables.Add(LabelName, LineNumber);
                    }

                }
                else
                    ShowFatalError("Lable instruction requires only instruction and lable name");

            }
        }

        public override void ExecuteInstruction()
        {

        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Label(Reference, Line);
        }
    }
    public class Jump : Instruction
    {
        string LableName;
        public Jump(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length == 2)
                    LableName = InstructionAndData[1];
                else
                    ShowFatalError("Jump Instruction requires instruction and label name");
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                if (programReference.Lables.ContainsKey(LableName))
                    programReference.JumpToInstruction(programReference.Lables[LableName]);
                else
                    ShowFatalError("no such label exists");
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Jump(Reference, Line);
        }
    }
    public class JumpEquals : Instruction
    {
        string labelname, var1, var2;
        public JumpEquals(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 2)
                    ShowFatalError(" jump equals requires instruction and its values");
                else
                {
                    string[] Operands = InstructionAndData[1].Split(',');
                    if (Operands.Length != 3)
                        ShowFatalError("jump equal instruction requires three values label,variable1,variable2");
                    else
                    {
                        labelname = Operands[0];
                        var1 = Operands[1];
                        var2 = Operands[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                int n1, n2;
                n1 = (int)GetDouble(var1);
                n2 = (int)GetDouble(var2);
                if (n1 == n2)
                {
                    if (programReference.Lables.ContainsKey(labelname))
                        programReference.JumpToInstruction(programReference.Lables[labelname]);
                    else
                        ShowFatalError(" no such label exists");
                }
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new JumpEquals(Reference, Line);
        }
    }
    public class JumpStringEquals : Instruction
    {
        string labelname, var1, var2;
        public JumpStringEquals(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 2)
                    ShowFatalError(" jump string equals requires instruction and its values");
                else
                {
                    string[] Operands = InstructionAndData[1].Split(',');
                    if (Operands.Length != 3)
                        ShowFatalError("jump string equal instruction requires three values label,variable1,variable2");
                    else
                    {
                        labelname = Operands[0];
                        var1 = Operands[1];
                        var2 = Operands[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                object n1 = GetData(var1);
                object n2 = GetData(var2);
                if (n1 is string && n2 is string)
                {
                    if (n1.ToString().Replace('"', ' ').TrimEnd().TrimStart() == n2.ToString().Replace('"', ' ').TrimEnd().TrimStart())
                    {
                        if (programReference.Lables.ContainsKey(labelname))
                            programReference.JumpToInstruction(programReference.Lables[labelname]);
                        else
                            ShowFatalError(" no such label exists");
                    }
                }
                else
                    ShowFatalError("jump string equals only works for string variables and data");
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new JumpStringEquals(Reference, Line);
        }
    }
    public class JumpGreaterThan : Instruction
    {
        string labelname, var1, var2;
        public JumpGreaterThan(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 2)
                    ShowFatalError(" jump greater than requires instruction and its values");
                else
                {
                    string[] Operands = InstructionAndData[1].Split(',');
                    if (Operands.Length != 3)
                        ShowFatalError("jump greater than instruction requires three values label,variable1,variable2");
                    else
                    {
                        labelname = Operands[0];
                        var1 = Operands[1];
                        var2 = Operands[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                double n1, n2;
                n1 = GetDouble(var1);
                n2 = GetDouble(var2);
                if (n1 > n2)
                {
                    if (programReference.Lables.ContainsKey(labelname))
                        programReference.JumpToInstruction(programReference.Lables[labelname]);
                    else
                        ShowFatalError(" no such label exists");
                }
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new JumpGreaterThan(Reference, Line);
        }
    }
    public class JumpGreaterThanEquals : Instruction
    {
        string labelname, var1, var2;
        public JumpGreaterThanEquals(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 2)
                    ShowFatalError(" jump greater than or equals requires instruction and its values");
                else
                {
                    string[] Operands = InstructionAndData[1].Split(',');
                    if (Operands.Length != 3)
                        ShowFatalError("jump greater than or equals instruction requires three values label,variable1,variable2");
                    else
                    {
                        labelname = Operands[0];
                        var1 = Operands[1];
                        var2 = Operands[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                double n1, n2;
                n1 = GetDouble(var1);
                n2 = GetDouble(var2);
                if (n1 >= n2)
                {
                    if (programReference.Lables.ContainsKey(labelname))
                        programReference.JumpToInstruction(programReference.Lables[labelname]);
                    else
                        ShowFatalError(" no such label exists");
                }
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new JumpGreaterThanEquals(Reference, Line);
        }
    }
    public class JumpLessThan : Instruction
    {
        string labelname, var1, var2;
        public JumpLessThan(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 2)
                    ShowFatalError(" jump less than requires instruction and its values");
                else
                {
                    string[] Operands = InstructionAndData[1].Split(',');
                    if (Operands.Length != 3)
                        ShowFatalError("jump less than instruction requires three values label,variable1,variable2");
                    else
                    {
                        labelname = Operands[0];
                        var1 = Operands[1];
                        var2 = Operands[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                double n1, n2;
                n1 = GetDouble(var1);
                n2 = GetDouble(var2);
                if (n1 < n2)
                {
                    if (programReference.Lables.ContainsKey(labelname))
                        programReference.JumpToInstruction(programReference.Lables[labelname]);
                    else
                        ShowFatalError(" no such label exists");
                }
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new JumpLessThan(Reference, Line);
        }
    }
    public class JumpLessThanEquals : Instruction
    {
        string labelname, var1, var2;
        public JumpLessThanEquals(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 2)
                    ShowFatalError(" jump less than or equals requires instruction and its values");
                else
                {
                    string[] Operands = InstructionAndData[1].Split(',');
                    if (Operands.Length != 3)
                        ShowFatalError("jump less than or equals instruction requires three values label,variable1,variable2");
                    else
                    {
                        labelname = Operands[0];
                        var1 = Operands[1];
                        var2 = Operands[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                double n1, n2;
                n1 = GetDouble(var1);
                n2 = GetDouble(var2);
                if (n1 <= n2)
                {
                    if (programReference.Lables.ContainsKey(labelname))
                        programReference.JumpToInstruction(programReference.Lables[labelname]);
                    else
                        ShowFatalError(" no such label exists");
                }
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new JumpLessThanEquals(Reference, Line);
        }
    }
    public class Input : Instruction
    {
        string Var;
        public Input(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 2)
                    ShowFatalError(" input instruction requires input instruction and variable name");
                else
                    Var = InstructionAndData[1];
            }
        }

        public override void ExecuteInstruction()
        {
            SetData(Var, Console.ReadLine());
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Input(Reference, Line);
        }
    }
    public class GetType : Instruction
    {
        string Var;
        public GetType(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length == 2)
                    Var = InstructionAndData[1];
                else
                    ShowFatalError("get type Instruction requires instruction and variable");
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                object Datas = GetData(Var);
                programReference.DataStack.Push(Datas.GetType().ToString());
            }
            catch (Exception error)
            {
                ShowFatalError(error.StackTrace);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new GetType(Reference, Line);
        }
    }
    public class Return : Instruction
    {
        public Return(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 1)
                    ShowFatalError("print instruction requires only instruction and print string or variable");
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                programReference.JumpToInstruction(programReference.ProgramStack.Pop());
            }
            catch (Exception error)
            {
                ShowFatalError("Program halted");
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Return(Reference, Line);
        }
    }
    public class Call : Instruction
    {
        string LableName;
        public Call(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length == 2)
                    LableName = InstructionAndData[1];
                else
                    ShowFatalError("Call Instruction requires instruction and label name");
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                if (programReference.Lables.ContainsKey(LableName))
                {
                    programReference.ProgramStack.Push(programReference.GetInstructionPointer());
                    programReference.JumpToInstruction(programReference.Lables[LableName]);
                }
                else
                    ShowFatalError("no such label exists");
            }
            catch (Exception error)
            {
                ShowFatalError(error.StackTrace);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new Call(Reference, Line);
        }
    }
    public class StrLen : Instruction
    {
        string Var;

        public StrLen(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                if (InstructionAndData.Length != 2)
                    ShowFatalError("strlen instruction requires only instruction and string or variable");
                else
                    Var = InstructionAndData[1];
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                string Object = "";
                if (IsString(Var))
                {
                    Object = Var.ToString().Remove(0, 1).Remove(Var.ToString().Length - 2, 1);
                    programReference.DataStack.Push(Object.Length);
                }
                else
                {
                    if (programReference.DataSegment.ContainsKey(Var))
                    {
                        object stringData = programReference.DataSegment[Var];
                        if (stringData != null)
                        {
                            if (IsString(stringData.ToString()))
                                Object = stringData.ToString().Remove(0, 1).Remove(stringData.ToString().Length - 2, 1);
                            else
                                Object = stringData.ToString();
                            programReference.DataStack.Push(Object.Length);
                        }
                        else
                            ShowFatalError("no data in the variable");
                    }
                    else if (Var.TrimStart().TrimEnd() == "stack")
                    {
                        Object = programReference.DataStack.Pop() + "";
                        programReference.DataStack.Push(Object.Length);
                    }
                    else
                        ShowFatalError("no such Variable as \"" + Var + "\"");
                }
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new StrLen(Reference, Line);
        }
    }
    public class StrCat : Instruction
    {
        string var1 = "", var2 = "";
        public StrCat(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                string[] values = InstructionAndData[1].Split(',');

                if (InstructionAndData.Length != 2)
                    ShowFatalError("strcat instruction requires only instruction and variables or data");
                else
                {
                    if (values.Length != 2)
                        ShowFatalError("strcat instruction requires two values var1,var2");
                    else
                    {
                        var1 = values[0];
                        var2 = values[1];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                string s1, s2;
                s1 = GetData(var1).ToString();
                s2 = GetData(var2).ToString();
                s1 = s1.Replace('"', ' ').TrimEnd().TrimStart();
                s2 = s2.Replace('"', ' ').TrimEnd().TrimStart();
                s1 += s2;
                programReference.DataStack.Push(s1);
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new StrCat(Reference, Line);
        }
    }
    public class StrSubString : Instruction
    {
        string var, start, length;
        public StrSubString(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                string[] values = InstructionAndData[1].Split(',');

                if (InstructionAndData.Length != 2)
                    ShowFatalError("substr instruction requires only instruction and variables or data");
                else
                {
                    if (values.Length != 3)
                        ShowFatalError("substr instruction requires three values var,start,length");
                    else
                    {
                        var = values[0];
                        start = values[1];
                        length = values[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                int startIndex = 0, Length = 0;
                string s = GetData(var).ToString();
                startIndex = (int)GetDouble(start);
                Length = (int)GetDouble(length);

                string ss = s.Substring(startIndex, Length);
                programReference.DataStack.Push(ss);
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }

        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new StrSubString(Reference, Line);
        }
    }
    public class FileIO : Instruction
    {
        string Var, Mode, Path;

        public FileIO(ProgramClass reference = null, string Line = null) : base(reference)
        {
            if (reference != null && Line != null)
            {
                string[] InstructionAndData = Line.Split(" ".ToCharArray(), 2);
                string[] values = InstructionAndData[1].Split(',');

                if (InstructionAndData.Length != 2)
                    ShowFatalError("file instruction requires only instruction and variables or data");
                else
                {
                    if (values.Length != 3)
                        ShowFatalError("file instruction requires three values Variable,Mode,Path");
                    else
                    {
                        Var = values[0];
                        Mode = values[1];
                        Path = values[2];
                    }
                }
            }
        }

        public override void ExecuteInstruction()
        {
            try
            {
                if (Mode == "read")
                {
                    string FilePath = GetData(Path).ToString().Replace('"', ' ').TrimEnd().TrimStart();
                    if (File.Exists(FilePath))
                    {
                        string FileData = File.ReadAllText(FilePath, Encoding.ASCII);
                        SetData(Var, FileData);
                    }
                    else
                        ShowFatalError("File does not exists");
                }
                else
                {
                    string Data = GetData(Var).ToString();
                    string FilePath = GetData(Path).ToString().Replace('"', ' ').TrimEnd().TrimStart();

                    if (Mode == "write")
                        File.WriteAllText(FilePath, Data);
                    else if (Mode == "append")
                    {
                        if (!File.Exists(FilePath))
                            File.WriteAllText(FilePath, "");
                        File.AppendAllText(FilePath, Data);
                    }
                    else
                        ShowFatalError("Invalid File Mode");
                }
            }
            catch (Exception error)
            {
                ShowFatalError(error.Message);
            }
        }

        public override Instruction GetNewInstruction(ProgramClass Reference, string Line)
        {
            return new FileIO(Reference, Line);
        }
    }
}
