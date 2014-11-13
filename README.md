Tech-F
======
General Purpose Utility Framework
---------------------------------

Currently only contains a class factory implementation which generates IL dynamically 
to enable the fastest available performance when resolving objects. The generated code
handles constructor injection and property binding automatically, and allows an entire
application and its dependencies be loaded automatically using app.config or web.config
or programmatically.

**Three example projects are provided which demonstrates is usage**

**TechF.Example.BasicUsage** demonstrates simple programmatic usage, with constructor
injection and property binding.

**TechF.Examples.BasicUsageConfiguration** demonstrates app.config based factory configuration
with constructor injection and property binding

**TechF.Examples.BasicWebApi** demonstrates using the class factory to implement IDependencyResolver
and allow the class factory to construct WebApi controllers with constructor
injection and property binding


