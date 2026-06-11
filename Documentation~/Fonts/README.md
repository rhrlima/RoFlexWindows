# Pixel Font configuration

Creation of the .ttf was done in the following website:
[https://yal.cc/tools/pixel-font/](https://yal.cc/tools/pixel-font/)

## RO-custom-regular

Metadata
| Param      | Value             |
| ---------- | ----------------- |
| Font Name  | RO-custom-regular |
| Font Style | Regular           |
| Author(s)  | rhrlima           |
| Version    | 2.0               |
| Description | Ragnarok Online inspired font. |
| License | MIT |
| License URL | https://opensource.org/license/mit |

### Characters

Minimum (27x6)
```
abcdefghijklmnopqrstuvwxyzç
ABCDEFGHIJKLMNOPQRSTUVWXYZÇ
 0123456789<>[]{}()\/
.,:;~`'"!?@#$%^&*-_=+|
áéíóúàèìòùâêîôûäëïöüãõñ
ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÄËÏÖÜÃÕÑ
```

Custom (26x7)
```
abcdefghijklmnopqrstuvwxyzç
ABCDEFGHIJKLMNOPQRSTUVWXYZÇ
 0123456789<>[]{}()\/
.,:;~`'"!?@#$%^&*-_=+|
áéíóúàèìòùâêîôûäëïöüÿãõñ
ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÄËÏÖÜŸÃÕÑ 
¿¡…□∎
```

Extended (MxN)
```
abcdefghijklmnopqrstuvwxyzç
ABCDEFGHIJKLMNOPQRSTUVWXYZÇ
 0123456789<>[]{}()\/!?¡¿
.,:;~`'"@#$%^&*-_=+|
áéíóúàèìòùâêîôûäëïöüãõñÿ
ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÄËÏÖÜÃÕÑŸ
ßœŒæÆ
$€£¥₩₽₹₫₴₲₱¢
…□∎
±×÷≠≈∞∑∆∫√°µλΩ
<>≤≥πφστθ
©®™‰†‡§¶¨¯˚ˇ˘¸¹²³¼½¾
```

### Input Parameters

| Param        | Value  |
| ------------ | ------ |
| Glyph color  | `auto` |
| Tile Width   | 11     |
| Tile Height  | 15     |
| Offset X     | 0      |
| Offset Y     | 0      |
| Separation X | 0      |
| Separation Y | 0      |
| Baseline X   | 0      |
| Baseline     | 12     |
| Spacing      | 2      |

### Output Parameters

| Param        | Value |
| ------------ | ----- |
| Em Size      | 1024  |
| Line gap     | 0     |
| Ascent       | 820   |
| Descent      | -204  |
| Pixel Size   | 64    |
| Contour Type | `Pixel` |
| Font Type    | `TTF`   |

### Unity Asset details

| Param | Value |
| - | - |
| Descent line | -4 |
| Underline Offset | -2 |
| Underline Thickness | 1 |
| Strikethrough Offset | 3 |
| Shader | TextMeshPro/Bitmap |
| Resolution | 256x256 |
| Characters | Extended ASCII |
| Render Mode | RASTER_HINTER / RASTER |