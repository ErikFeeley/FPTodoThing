module Page.Todo exposing (Model, Msg, init, update, view)

import Html exposing (Html, div, text)
import Http
import Todo exposing (Todo)



-- MODEL


type Model
    = Failure Http.Error
    | Loading
    | Success Todo


init : Int -> ( Model, Cmd Msg )
init todoId =
    ( Loading, Todo.fetchOne GotTodo todoId )



-- VIEW


view : Model -> Html msg
view model =
    div [] [ Debug.toString model |> text ]



-- UPDATE


type Msg
    = GotTodo (Result Http.Error Todo)


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        GotTodo (Ok value) ->
            ( Success value, Cmd.none )

        GotTodo (Err httpError) ->
            ( Failure httpError, Cmd.none )
