module Layout exposing (view)

import Browser exposing (Document)
import Html exposing (Html)



-- VIEW


view : (a -> msg) -> Html a -> Document msg
view toMsg html =
    { title = "Todos"
    , body =
        [ html
            |> Html.map toMsg
        ]
    }
