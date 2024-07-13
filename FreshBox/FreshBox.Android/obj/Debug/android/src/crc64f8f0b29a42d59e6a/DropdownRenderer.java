package crc64f8f0b29a42d59e6a;


public class DropdownRenderer
	extends crc643f46942d9dd1fff9.ViewRenderer_2
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("FreshBox.Droid.DropdownRenderer, FreshBox.Android", DropdownRenderer.class, __md_methods);
	}


	public DropdownRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == DropdownRenderer.class) {
			mono.android.TypeManager.Activate ("FreshBox.Droid.DropdownRenderer, FreshBox.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
		}
	}


	public DropdownRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == DropdownRenderer.class) {
			mono.android.TypeManager.Activate ("FreshBox.Droid.DropdownRenderer, FreshBox.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public DropdownRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == DropdownRenderer.class) {
			mono.android.TypeManager.Activate ("FreshBox.Droid.DropdownRenderer, FreshBox.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
		}
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
