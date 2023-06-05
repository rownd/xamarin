namespace Rownd.Store

type SetAuthStateAction = { AuthState: AuthState }
//type DecrementAction = { Amount: int }

type Actions =
    | SetAuthState of SetAuthStateAction
    //| Decrement of DecrementAction
