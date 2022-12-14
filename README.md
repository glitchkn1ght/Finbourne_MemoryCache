General Design
- Caching and multithreading are not areas of programming i am familiar with, either from personal projects or professionally. Therefore i wasn't really sure how to approach the task and went back and forth on the design with it
  taking around 12 hours overall. All feedback is very appreciated as i'd like to learn more about both areas.
- The spec mentions an in-memory cache component, therefore to start with i used the MemoryCache class from the Microsoft.Extensions.Caching package. However after doing some research there didn't seem to be much 
  advantage to using MemoryCache over a concurrentDictionary aside from features like expiration policies which the spec did not mention being needed. 
- I thought it would be useful for clients of the cache to have feedback that was in a consistent format and for each operation rather than just for the eviction method. Hence returning status codes/messages which each client
  can decide how to use. 
- The spec mentions a configurable threshold for the number of items i implemented this via IOptionsMonitor which allow values to be read and re-read from a config file. (Although i'm not sure i configured mine correctly to do the latter) 
- In an attempt to make the component thread safe i registered the cache and it's wrapper class as a singletons in the DI (and is the reason for using concurrentDictionary over a normal dictionary object). However as it's a new
  area for me i'm far from certain that this is bulletproof.  
- I wanted to keep the core operations (adding, deleting, retrieving from the cache) seperate from other concerns (input validation/eviction policy) to allow the latter to be more easily updated if required. Hence having
  the cacheWrapper class. 

Areas for improvement
- Make sure all methods/classes are thread safe. 
- Any non null object is accepted into the cache, including ones which are almost certainly invalid such as whitespace only strings. Really I'd like abstract the validation into its own class as its bulking out the wrapper.
- Exhaustive unit/integration testing. 
- As is it is possible to exceed the cache limit as the eviction is a separate operation - which can fail - that takes place after the addition of the new object. Ideally there should be some sort of rollback functionality
  similiar to transactions in sql server e.g. either all of it succeeds or none of it. 
- The firstOrDefault search in the eviction method would be inefficient in a big cache. 