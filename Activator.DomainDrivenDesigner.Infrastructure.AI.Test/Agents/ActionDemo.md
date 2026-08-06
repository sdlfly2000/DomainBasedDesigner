## Generate complete C# code 
File: **Domain/User/UserService.cs**

## Rules:
1. Class: **UserService**

2. Inject below through constructor
- **IUserRepository**

3. Public async method signature: ```Task<User> GetUser(Guid Id)```
    
    Method **GetUser** logic: 
    ```mermaid
        flowchart TB
            subgraph main [Domain.User.UserService.GetUser]
                direction TB
                start(("Start -> [UserId: Guid]")) -->
                FindUser["`Find User By *UserId* -> [IUserRepository.GetUserById(Id)]`"] -->
                check1{Found?} --"no"-->
                userNotFound["`Throw *DomainNotFoundException* exception`"]      
                check1 --""yes""--> 
                return["`Return **User**`"]
            end
    ```
4. Public async method signature: ```Task<bool> UpsertUser(User user)```

    Method **UpsertUser** logic: 
    ```mermaid
        flowchart TB
            subgraph main [Domain.User.UserService.UpsertUser]
                direction TB
                start(("Start -> [User: User]")) -->
                FindUser["`Find User By *User.Id* -> [IUserRepository.GetUserById(User.Id)]`"] -->
                check1{Found?} --"no"-->
                userNotFound["`Create a **User** -> [IUserRepository.Create(User)]`"] --> return     
                check1 --"yes"--> 
                map["`Map **User** -> [IUserRepository.Map(user, ref existingUser)]`"] -->
                save["`Save Mapped **User** -> [IUserRepository.Save(mappedUser)]]`"] -->
                return("`Return **User**`")
            end
    ```

## Reference Interfaces:
public interface IUserRepository
- Task<User> GetUserById(Guid Id);
- Task<User> UpdateUser(User user);
- Task<User> CreateUser(User user);
- Task<bool> Save(User user);
- void Map(User source, ref User destination);

## Output
Only output full source code of **UserService.cs** in proper format, no other text. Use async/await correctly and Use ConfigureAwait(false) for each async call.