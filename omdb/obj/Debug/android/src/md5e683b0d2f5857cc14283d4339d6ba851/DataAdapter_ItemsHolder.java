package md5e683b0d2f5857cc14283d4339d6ba851;


public class DataAdapter_ItemsHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MoviesDirectory.Adapter.DataAdapter+ItemsHolder, omdb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DataAdapter_ItemsHolder.class, __md_methods);
	}


	public DataAdapter_ItemsHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DataAdapter_ItemsHolder.class)
			mono.android.TypeManager.Activate ("MoviesDirectory.Adapter.DataAdapter+ItemsHolder, omdb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	java.util.ArrayList refList;
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
