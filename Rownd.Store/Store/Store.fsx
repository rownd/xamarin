namespace Rownd.Store

#r "nuget: redux-fsharp, 1.0.0"
open Redux.Store
open Redux.CombineReducers

module Store = 
    let authReducer state action =
        match action with
        | SetAuthState { AuthState = authState } -> { state with AuthState = authState }
        //| Decrement { Amount = amount } -> { state with CurrentValue = state.CurrentValue - amount }

    let reducer = combineReducers [|authReducer|]

    let defaultEnhancer = id

    let createStore state =
        createStore reducer state id
