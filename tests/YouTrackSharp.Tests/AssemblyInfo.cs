using Xunit;

// GH-129 Should we allow parallelization to run tests faster?
[assembly: CollectionBehavior(DisableTestParallelization = true)]