# FeSimpleHelpersV2
This is a unity project containing convenient scripts and assets for starting a (not that big) single player game. </p>
Basically a series of scripts and project strcuture hardened over the years. </p>
It's intention is to remain simple but with most commonly used **stuff**.

# How to use
- Create new Unity project with last LTS Unity version.
- Download this repo as a zip.
- Paste all content in Assets folder to your project's Asset folder.
- Probably fix any issues that might arise about Unity updating its systems.
- Replace everywhere you see "MyGame" to your game name (changing namespaces would be wise also).

That's it. It's yours now. Do whatever you want with it.
  
# Why is it not a package
Simplicity, just download all, modify at will, remove at will. </p>
As its intended to import only on project creation, don't care for backwards compatibility.

# Things to consider
- Subsequent updates **will not care** for backwards compatibility.
- Does not use Assembly Definitions. Take care of that in any way you want.
- Doesn't care about Addressables and the sorts.
- SFX Manager is old. Been using FMOD lately

# How is it structured
It is centered around various unique Scriptable Objects (class ConfigSingleton). </p>
Check files folder Scripts/FeSimpleHelpers/Runtime/Core. </p>
Check Resources Folder, specially Resources/Main Configs. </p>
Might add more info after testing with users.

# Future updates:
- Polish FSM Node system. Also adding examples.
- Add a FX Manager with pools.
