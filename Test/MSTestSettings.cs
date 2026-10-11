using Microsoft.VisualStudio.TestTools.UnitTesting;

// Cada prueba usa sus propios Fakes en memoria, así que pueden correr en paralelo sin molestarse.
[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]
