module Main exposing (main)

import Browser exposing (Document, UrlRequest, application)
import Browser.Navigation as Nav exposing (Key)
import Html exposing (div)
import Layout
import Page.Todo as Todo
import Page.Todos as Todos
import Url exposing (Url)
import Url.Parser as Parser exposing ((</>), Parser, int, oneOf, s)



-- MAIN


main : Program () Model Msg
main =
    application
        { init = init
        , view = view
        , update = update
        , subscriptions = subscriptions
        , onUrlChange = UrlChanged
        , onUrlRequest = LinkClicked
        }



-- MODEL


type alias Model =
    { page : Page
    , key : Key
    }


type Page
    = NotFound
    | Blank
    | Todos Todos.Model
    | Todo Todo.Model


init : () -> Url -> Key -> ( Model, Cmd Msg )
init _ url key =
    toRoute url { key = key, page = Blank }



-- VIEW


view : Model -> Document Msg
view model =
    case model.page of
        NotFound ->
            div [] [ Html.text "not found" ]
                |> Layout.view never

        Blank ->
            div [] []
                |> Layout.view never

        Todos subModel ->
            Todos.view subModel
                |> Layout.view GotTodosMsg

        Todo subModel ->
            Todo.view subModel
                |> Layout.view GotTodoMsg



-- UPDATE


type Msg
    = LinkClicked UrlRequest
    | UrlChanged Url
    | GotTodosMsg Todos.Msg
    | GotTodoMsg Todo.Msg


routeParser : Model -> Parser (( Model, Cmd Msg ) -> a) a
routeParser model =
    oneOf
        [ Parser.map (updateWith Todos GotTodosMsg model Todos.init) Parser.top
        , Parser.map (\todoId -> updateWith Todo GotTodoMsg model (Todo.init todoId)) (s "todo" </> int)
        ]


toRoute : Url -> Model -> ( Model, Cmd Msg )
toRoute url model =
    Parser.parse (routeParser model) url
        |> Maybe.withDefault ( { model | page = NotFound }, Cmd.none )


updateWith : (subModel -> Page) -> (subMsg -> Msg) -> Model -> ( subModel, Cmd subMsg ) -> ( Model, Cmd Msg )
updateWith toPage toMsg model ( subModel, subMsg ) =
    ( { model | page = toPage subModel }
    , Cmd.map toMsg subMsg
    )


updateWithNoCmd : (subModel -> Page) -> Model -> subModel -> ( Model, Cmd Msg )
updateWithNoCmd toPage model subModel =
    ( { model | page = toPage subModel }, Cmd.none )


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case ( msg, model.page ) of
        ( LinkClicked urlRequest, _ ) ->
            case urlRequest of
                Browser.Internal url ->
                    ( model, Nav.pushUrl model.key (Url.toString url) )

                Browser.External href ->
                    ( model, Nav.load href )

        ( UrlChanged url, _ ) ->
            toRoute url model

        ( GotTodosMsg subMsg, Todos subModel ) ->
            Todos.update subMsg subModel
                |> updateWith Todos GotTodosMsg model

        ( GotTodosMsg subMsg, _ ) ->
            ( model, Cmd.none )

        ( GotTodoMsg subMsg, Todo subModel ) ->
            Todo.update subMsg subModel
                |> updateWith Todo GotTodoMsg model

        ( GotTodoMsg subMsg, _ ) ->
            ( model, Cmd.none )



-- SUBSCRIPTIONS


subscriptions : Model -> Sub Msg
subscriptions model =
    Sub.none
