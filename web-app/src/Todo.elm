module Todo exposing (Todo, fetchAll, fetchOne)

import Http
import Json.Decode as Decode exposing (Decoder, bool, float, int, string)
import Json.Decode.Pipeline exposing (hardcoded, optional, required)
import Json.Encode as Encode
import Url.Builder as Builder


type alias Todo =
    { todoId : Int
    , description : String
    , isActive : Bool
    }


todoDecoder : Decoder Todo
todoDecoder =
    Decode.succeed Todo
        |> required "todoId" int
        |> required "description" string
        |> required "isActive" bool


todoEncoder : Todo -> Encode.Value
todoEncoder todo =
    Encode.object [ ( "description", Encode.string todo.description ) ]


fetchAll : (Result Http.Error (List Todo) -> msg) -> Cmd msg
fetchAll toMsg =
    Http.get
        { url = Builder.crossOrigin "https://localhost:5001" [ "api", "todos" ] []
        , expect = Http.expectJson toMsg <| Decode.list todoDecoder
        }


fetchOne : (Result Http.Error Todo -> msg) -> Int -> Cmd msg
fetchOne toMsg todoId =
    Http.get
        { url = Builder.crossOrigin "https://localhost:5001" [ "api", "todos", String.fromInt todoId ] []
        , expect = Http.expectJson toMsg <| todoDecoder
        }
