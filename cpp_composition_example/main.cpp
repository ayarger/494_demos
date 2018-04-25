#include <typeinfo>
#include <iostream>
#include <vector>

#include "headers/Entity.h"
#include "headers/Component.h"
#include "headers/Renderer.h"
#include "headers/Transform.h"

int main() {
    std::cout << "[Beginning Example]" << std::endl;
    
    Entity e;
    
	e.AddComponent<Renderer>();
    e.AddComponent<Transform>();
	
	Transform* entity_transform = e.GetComponent<Transform>();
	entity_transform-> SetPosition(Point3{1, 2, 3});
	std::cout << "Transform x: " << entity_transform->GetPosition().x << std::endl;
	std::cout << "Transform y: " << entity_transform->GetPosition().y << std::endl;
	std::cout << "Transform z: " << entity_transform->GetPosition().z << std::endl;
    
    // 15 Updates
    e.Update();
    
    std::cout << "[End of Example]" << std::endl;
}
