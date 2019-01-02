<Query Kind="Program" />

void Main()
{
	Program.Test();
}

// Contravariant interface.
interface IContravariant<in A> {
	void DoSth(A arg);
}

// Extending contravariant interface.
interface IExtContravariant<in A> : IContravariant<A> { }

// Implementing contravariant interface.
class Sample<A> : IContravariant<A> {
	public void DoSth(A arg)
	{
		Console.WriteLine(typeof(A).Name);
		Console.WriteLine(arg.GetType().Name);
	}
}

class Program
{
    public static void Test()
    {
        IContravariant<Object> iobj = new Sample<Object>();
        IContravariant<String> istr = new Sample<String>();

        // You can assign iobj to istr because
        // the IContravariant interface is contravariant.
        istr = iobj;
		
		iobj.DoSth(new object());
		istr.DoSth("Test");
    }
}
