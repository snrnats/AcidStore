# AcidStore
A sample project to tackle ACID storage implementation problems.

# Problems
Given a storage that implement an interface:

```
interface IStorage
{
    void BeginTransaction();
    void Commit();
    void Abort();


    void Create(int id, object obj);    
    T GetAll();
    T Get(int id);
    void Update(int id, object obj);
    void Delete(int id);
}
```

Then make sure that concurrency problems such as Dirty reads/writes, read/write skew, lost update, and phantom reads can be resolved using transactions.

## Dirty reads

```mermaid
sequenceDiagram
    participant A
    participant Storage
    Note over Storage: x = 2
    participant B

    A->>Storage: set x = 3
    activate Storage
    Storage-->>A: ok
    deactivate Storage

    B->>Storage: get x
    activate Storage
    Storage-->>B: 3
    deactivate Storage
    Note left of B: It must be 2

    A->>Storage: commit
    activate Storage
    Storage-->>A: ok
    deactivate Storage
    
    B->>Storage: get x
    activate Storage
    Storage-->>B: 3
    deactivate Storage
```

## Dirty writes

```mermaid
sequenceDiagram
    participant A
    participant Storage
    participant B

    A->>Storage: set x = 3
    activate Storage
    Storage-->>A: ok
    deactivate Storage

    B->>Storage: set x = 4
    activate Storage
    Storage-->>B: ok
    deactivate Storage
    Note left of B: It must be delayed until A commit

    A->>Storage: commit
    activate Storage
    Storage-->>A: ok
    deactivate Storage
    
    B->>Storage: get x
    activate Storage
    Storage-->>B: 3
    deactivate Storage
    
    Note left of B: It must be 4
```

## Read skew (nonrepeatable read)

```mermaid
sequenceDiagram
    participant A
    participant Storage
    Note over Storage: balance 1 = 500 <br/> balance 2 = 500
    participant B

    A->>Storage: select balance <br/> where account = 1
    activate Storage
    Storage-->>A: 500
    deactivate Storage

    B->>Storage: set balance = balance + 100 <br/> where account = 1
    activate Storage
    Storage-->>B: ok
    deactivate Storage

    B->>Storage: set balance = balance - 100 <br/> where account = 2
    activate Storage
    Storage-->>B: ok
    deactivate Storage

    B->>Storage: commit
    activate Storage
    Storage-->>B: ok
    deactivate Storage

    A->>Storage: select balance <br/> where account = 2
    activate Storage
    Storage-->>A: 400
    deactivate Storage

    A->>Storage: commit
    activate Storage
    Storage-->>A: ok
    deactivate Storage
    
    Note right of A: Total is 900 <br/> Must be 1000
```