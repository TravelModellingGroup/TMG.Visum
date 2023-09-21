global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using TMG.Visum;

// Assuming most people have at least 8 cores, we are still bottle necked
// with loading VISUM.
[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]


