#ifndef COMPONENT_H
#define COMPONENT_H

class Component {
    public:
		/* Executes after added to entity */
        virtual void Start() {}
		
		/* Executes after every frame */
        virtual void Update() {}
		
		/* Executes on destruction of component or parent entity */
		virtual void Destroy() {}
};

/* TODO */
// Restrict Components to creation-by-factory to allow for better tracking of component life cycle?
// 

#endif
