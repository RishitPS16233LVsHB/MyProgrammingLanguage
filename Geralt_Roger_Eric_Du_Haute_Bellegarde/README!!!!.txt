you should be running Geralt_Roger_Eric_Du_Haute_Bellegarde.exe for compiling and running Geralt_Roger_Eric_Du_Haute_Bellegarde scripts
if you are using this executable please do not change the executable name, come on dont do it!!!

below is the instructions and guidelines of writing Geralt_Roger_Eric_Du_Haute_Bellegarde script:- 

-this language is built on top of C# DotNET so you will see a lot of dependency and this is my second try on making a language

-here is the program structure:-
	-DataArray(accessed using keyword array@<your int index here>)
	-DataStack(accessed using keyword stack)
	-CodeSegment

-in Geralt_Roger_Eric_Du_Haute_Bellegarde the instructions are terminated using your favourite ';'

-here are the basic datatypes supported by the language
	-string(should be encoded in "")
	-int
	-double

-try to use halt keyword whenever you think you want to stop the program or it will show you message to explicitly stop the program

-to declare a variable on program memory declare here is the syntax:-
	-declare string/int/double,varname/stack/array,<some value from variable,stack,array or just value>;
	-if you use declare for array index will be returned for that new variable on array;

-you can assign values using assign instruction here is the syntax:- 
	assign <stack,array@<your int index here>,var>,<stack,array@<your int index here>,var,value>;

-arithmetic operations can be done using instructions add,sub,mul,div and mod syntax is:- 
	add/sub/mul/div/mod <stack,array@<your int index here>,var>assigning variable,<stack,array@<your int index here>,var> var1,<stack,array@<your int index here>,var>var2;

-to print anything to console use print instruction syntax is:-
	print <stack,array@<your int index here>,var>printing variable or printing value;

-to take console input use input instruction syntax is:- 
	input <stack,array@<your int index here>,var>assigning variable;

-to perform conditionals you can use:-
	-jump_equals
	-jump_string_equals(to check if two strings are matching or not)
	-jump_greater_than
	-jump_greater_than_or_equals
	-jump_less_than
	-jump_less_than_or_equals

-to perform conditionals you need also labels:-
	labels <label name(not in string '"')>;

-all of the jump instructions for exception of jump have three operands
	-labelname
	-<stack,array@<your int index here>,var>variable1
	-<stack,array@<your int index here>,var>variable2

	-jump_equals label,var1,var2;

- jump instruction only requires label name


-you can create procedures
	- procedures in Geralt_Roger_Eric_Du_Haute_Bellegarde is lets just say a bit ridiculous
	- to understand procedures keep in mind that program execution starts from first line and always goes to the last most line meaning if you have a procedure code anywhere it will be executed 
	  but you can prevent it by writing all your procedure code after the halt instruction meaning all the code before halt will be your main code and after it will be secondary or procedure code
	- in Geralt_Roger_Eric_Du_Haute_Bellegarde scripts procedures can be created by using labels too cause i am lazy AF XD......
	- label can be called and current instruction will be pushed to the program's call stack.
	- to return to the previous code we shall use return keyword 
	- always use return keyword at the end of your procedure code	
	- to call procedure use call instruction which can be use as:- call <label_name_here>;
	-using return keyword without calling any procedure will terminate program

	-beware though all data is global and so are the labels
		-if you are in a procedure code and jump to a label which is outside of the procedure program will jump to that label and program's call stack will not be poped
		-meaning if you use return keyword in that instance the program will return not to the previous procedure but to its procedure call meaning "incorrect return" will happen.

-file io
	- file io can be done using file instruction
	- file 	<stack,array@<your int index here>,var>variable,mode(read,write,append),<stack,array@<your int index here>,var,string value>path_variable;
	- if read then
		-var will be storing your file data
	- else for write and append
		-var's data will be stored in path if file not exists it will create new file as for directory not existing it will throw error

- string based operations
	-strlen instruction to get the length of string 
		-strlen <stack,array@<your int index here>,var>string variable;
		-length in int will pushed to data stack which can be used using stack keyword

	-strcat instruction to concatenate strings 
		-strcat <stack,array@<your int index here>,var>variable1,<stack,array@<your int index here>,var>variable2;
		-any int/double/string data will be concatenated as string and pushed to the stack;
	
	-substr instruction to get substring
		-substr <stack,array@<your int index here>,var>variable,<stack,array@<your int index here>,var>starting index in int,<stack,array@<your int index here>,var>ending index in int;
		-string variable's substring will be pushed to stack

- also you can provide commandline arguments after the script path and they will be stored in program's array 

- lastly get_type <stack,array@<your int index here>,var>variable; 
	- this will push type name of variable in string to the program's stack;
	- string will be System.String
	- int will be System.Int32
	- double will be System.Double

-the source code for the compiler is provided to in case any inconsistencies or issues or bugs you may add to it and you may add new instructions to the code just be sure to add the new instruction to instruction registry which is set in Instruction.SetRegistry() method

-also please see some of the sample programs

Thank you....:)



