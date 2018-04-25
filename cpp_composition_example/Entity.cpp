
#include <cstddef>
#include "Entity.h"

void Entity::Update() {
	int num_components = components.size();
	for(int i = 0; i < num_components; i++) {
		components[i]->Update();
	}
}
		
void Entity::Destroy() {
	int num_components = components.size();
	for(int i = 0; i < num_components; i++) {
		components[i]->Destroy();
	}
	
	for(int i = 0; i < num_components; i++) {
		delete components[i];
	}
}
		
Entity::~Entity() {
	int num_components = components.size();
	for(int i = 0; i < num_components; i++) {
		components[i]->Destroy();
	}
	
	for(int i = 0; i < num_components; i++) {
		delete components[i];
	}
}
        
