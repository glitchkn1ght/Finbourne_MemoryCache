General Design
- Caching and multithreading are not areas of programming i am familiar with, either from personal projects or professionally. Therefore i wasn't really sure how to approach the task and went back and forth on the design with it
  taking around 10 hours overall.
- The spec mentions an in-memory cache component, therefore i started with using this type the from Microsoft.Extensions.Caching package. However after doing some research on how to implement caches there didn't seem to be much 
  advantage to using this MemoryCache over a regular dictionary (which i am much more familiar with). Especially since the spec didn't mention using MemoryCaches built in features like expiration policies. 
  However i am aware i don't know much about the subject so i apologize if that is completely the wrong approach.  
- I thought it would be useful for clients of the cache to have feedback that was in a consistent format and for each operation rather than just for the eviction method. Hence returning status codes/messages which each client
  can decide how to use. 

Areas for improvement
- More than anything it needs to be easier to unit test. I tried to implement this I tried by refactoring so that cache itself would be in one class that could be abstracted with the and the get/methods in another class as a 
   wrapper. However since the class needs to be a singleton this did not work as you cannot have the getInstance constructor in the interface, i am not sure of a way around this. 
 
