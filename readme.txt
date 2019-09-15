COMP30019 Project 1


Camera motion:
"FlightSimulator.cs" is the script that controls camera motion including its orientation and position.
For preventing camera penetrating objects, "Rigidbody" and "BoxCllider/MeshCollider" are added to the camera, landscape and water.
Ref: http://forum.unity3d.com/threads/how-to-lock-or-set-the-cameras-z-rotation-to-zero.68932/#post-441968

Landscape:
"Landscape.cs" is the script that generate a landscape based on diamond-square algorithm. And the height of sand, grass and snow is differed in this script.
"LandscapeShader.shader" defined the landscape shader which implements the phong illumination model and phong shading technique.

 Water:
 "Water.cs" generate a water plane according to the dimensions of the landscape.
 "WaterShader.shader" focus on the wave the water surfac and also implements the phong illumination model.

Sun:
"Sun.cs" update the position of sun based on time changed.



