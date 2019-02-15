module Page.Todos exposing (Model, Msg, init, update, view)

import Html exposing (Html, div, text)
import Html.Attributes as Attributes exposing (href)
import Http
import Todo exposing (Todo)



-- MODEL


type Model
    = Failure Http.Error
    | Loading
    | Success (List Todo)


init : ( Model, Cmd Msg )
init =
    ( Loading, Todo.fetchAll GotTodos )



-- VIEW


view : Model -> Html msg
view model =
    div []
        [ Debug.toString model |> text
        , Html.a [ href "/todo/1" ] [ text "todo" ]
        ]



-- UPDATE


type Msg
    = GotTodos (Result Http.Error (List Todo))


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        GotTodos (Ok value) ->
            ( Success value, Cmd.none )

        GotTodos (Err httpError) ->
            ( Failure httpError, Cmd.none )
