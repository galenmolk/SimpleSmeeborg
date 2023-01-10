Shader "Unlit/Cell"
{
    Properties
    {
        _WallColor ("Wall Color", Color) = (0,0,0,1)
        _FloorColor ("Floor Color", Color) = (1,1,1,1)
        _WallThickness ("Wall Thickness", Range(0.03, 0.2)) = 0.1

        _MainTex ("Texture", 2D) = "white" {}

        [Toggle] _North ("North", Float) = 0
        [Toggle] _South ("South", Float) = 0
        [Toggle] _East ("East", Float) = 0
        [Toggle] _West ("West", Float) = 0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform fixed4 _WallColor;
            uniform fixed4 _FloorColor;

            uniform fixed _North;
            uniform fixed _South;
            uniform fixed _East;
            uniform fixed _West;

            uniform float _WallThickness;

            fixed getNorthPassability(fixed y)
            {
                return _North == 1 || y < 1 - _WallThickness;
            }

            fixed getSouthPassability(fixed y)
            {
                return _South == 1 || y > _WallThickness;
            }

            fixed getEastPassability(fixed x)
            {
                return _East == 1 || x < 1 - _WallThickness;
            }

            fixed getWestPassability(fixed x)
            {
                return _West == 1 || x > _WallThickness;
            }

            fixed getInterpolant(fixed x, fixed y)
            {
                return getNorthPassability(y) &&
                       getSouthPassability(y) &&
                       getEastPassability(x) &&
                       getWestPassability(x);
            }

            fixed4 getColor(fixed x, fixed y)
            {
                return lerp(_WallColor, _FloorColor, getInterpolant(x, y));
            }

            fixed4 frag (v2f_img i) : SV_Target
            {
                return getColor(i.uv.x, i.uv.y);
            }

            ENDCG
        }
    }
}
