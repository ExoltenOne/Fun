<Query Kind="Program" />

void Main()
{
	ModifyFixedStorage();
}

// The fixed statement prevents the garbage collector from relocating a movable variable. The fixed statement is only permitted in an unsafe context. fixed can also be used to create fixed size buffers.


class Point 
{ 
    public int x;
    public int y; 
}

unsafe private static void ModifyFixedStorage()
{
    // Variable pt is a managed variable, subject to garbage collection.
    Point pt = new Point();

    // Using fixed allows the address of pt members to be taken,
    // and "pins" pt so that it is not relocated.

    fixed (int* p = &pt.x)
    {
        *p = 1;
    }
}