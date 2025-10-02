shader_type spatial;
render_mode unshaded;

uniform sampler2D texture_albedo;
uniform float deformity;
uniform float offset;
uniform float brightness;
uniform vec3 uv1_scale;
uniform vec3 uv1_offset;

varying vec3 uv1_triplanar_pos;
uniform float uv1_blend_sharpness : hint_range(0.0, 150.0, 0.001);
varying vec3 uv1_power_normal;

vec4 triplanar_texture(sampler2D p_sampler, vec3 p_weights, vec3 p_triplanar_pos) {
	vec4 samp = vec4(0.0);
	samp += texture(p_sampler, p_triplanar_pos.xy) * p_weights.z;
	samp += texture(p_sampler, p_triplanar_pos.xz) * p_weights.y;
	samp += texture(p_sampler, p_triplanar_pos.zy * vec2(-1.0, 1.0)) * p_weights.x;
	return samp;
}

void vertex() {
	// Called for every vertex the material is visible on.
	
	// UV1 Triplanar: Enabled
	uv1_power_normal = pow(abs(NORMAL), vec3(uv1_blend_sharpness));
	uv1_triplanar_pos = VERTEX * uv1_scale + uv1_offset;
	uv1_power_normal /= dot(uv1_power_normal, vec3(1.0));
	uv1_triplanar_pos *= vec3(1.0, -1.0, 1.0);
	
	vec4 albedo_tex = triplanar_texture(texture_albedo, uv1_power_normal, uv1_triplanar_pos * TIME / 100.0);
	VERTEX *= offset + ((albedo_tex.r + albedo_tex.g + albedo_tex.b) / 3.f * deformity);
}

void fragment() {
	// Called for every pixel the material is visible on.
	vec4 albedo_tex = triplanar_texture(texture_albedo, uv1_power_normal, uv1_triplanar_pos * TIME / 100.0);
	ALBEDO = albedo_tex.rgb * brightness;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
