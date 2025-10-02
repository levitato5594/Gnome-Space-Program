shader_type spatial;
render_mode unshaded;

uniform vec3 colour = vec3(0,1,0);
uniform float maxAlpha = 0.8;
uniform float minAlpha = 0.2;
uniform float pulseSpeed = 2.5;
uniform float expand = 2.5;

void vertex() {
	// Called for every vertex the material is visible on.
	VERTEX *= expand;
}

void fragment() {
	// Called for every pixel the material is visible on.
	ALBEDO = colour;
	ALPHA = ((sin(TIME * pulseSpeed) + 1.0) / 2.0) * (maxAlpha - minAlpha) + minAlpha;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
