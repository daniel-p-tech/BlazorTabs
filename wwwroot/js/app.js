window.blazorTabs = {

    dropdownComponents: [],
    dynamicTabSetComponents: [],

    registerDynamicTabSetComponent: (div, component, componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        if (index === -1) {
            blazorTabs.dynamicTabSetComponents.push({ div: div, component: component, componentGuid: componentGuid });
            blazorTabs.setDynamicTabSetComponentHeight(componentGuid);
        }
    },

    setDynamicTabSetComponentHeight: (componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[index];
        let height = window.innerHeight - dynamicTabSetComponent.div.getBoundingClientRect().top;
        dynamicTabSetComponent.component.invokeMethodAsync('SetContentHeight', height);
    },

    getDynamicTabSetComponentHeight: (componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabSetComponents, componentGuid);
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[index];
        let height = window.innerHeight - dynamicTabSetComponent.div.getBoundingClientRect().top;
        return height;
    },

    registerDropdownComponent: (button, dropdownMenu, component, componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dropdownComponents, componentGuid);
        if (index === -1) {
            blazorTabs.dropdownComponents.push({ button: button, dropdownMenu: dropdownMenu, component: component, componentGuid: componentGuid });
        }
    },

    unregisterDropdownComponent: (componentGuid) => {
        blazorTabs.unregisterComponent(blazorTabs.dropdownComponents, componentGuid);
    },

    unregisterComponent: (components, componentGuid) => {
        let index = blazorTabs.findComponentIndex(components, componentGuid);
        if (index !== -1) {
            components.splice(index, 1);
        }
    },

    findComponentIndex: (components, componentGuid) => {
        let index = -1;
        for (let i = 0; i < components.length; i++) {
            if (components[i].componentGuid === componentGuid) {
                index = i;
                break;
            }
        }

        return index;
    },
}

window.addEventListener('mouseup', function (event) {
    for (let i = 0; i < blazorTabs.dropdownComponents.length; i++) {
        let dropdownComponent = blazorTabs.dropdownComponents[i];
        if (event.target !== dropdownComponent.button && event.target.parentNode !== dropdownComponent.button) {
            if (dropdownComponent.dropdownMenu.classList.contains('show')) {
                dropdownComponent.component.invokeMethodAsync('CloseDropdown');
            }
        }
    }
});

window.addEventListener("resize", function () {
    for (let i = 0; i < blazorTabs.dynamicTabSetComponents.length; i++) {
        let dynamicTabSetComponent = blazorTabs.dynamicTabSetComponents[i];
        blazorTabs.setDynamicTabSetComponentHeight(dynamicTabSetComponent.componentGuid);
    }
});