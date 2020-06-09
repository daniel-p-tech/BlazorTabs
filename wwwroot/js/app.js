window.blazorTabs = {

    dropdownComponents: [],
    dynamicTabContentComponents: [],

    registerDynamicTabContentComponent: (div, component, componentGuid) => {
        let index = blazorTabs.findComponentIndex(blazorTabs.dynamicTabContentComponents, componentGuid);
        if (index === -1) {
            blazorTabs.dynamicTabContentComponents.push({ div: div, component: component, componentGuid: componentGuid });
        }
    },

    unregisterDynamicTabComponent: (componentGuid) => {
        blazorTabs.unregisterComponent(blazorTabs.dynamicTabContentComponents, componentGuid);
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
        let index = blazorTabs.findComponentIndex(blazorTabs.dropdownComponents, componentGuid);
        if (index !== -1) {
            components.splice(index, 1);
        }
    },

    findComponentIndex: (components, componentGuid) => {
        let index = -1;
        for (let i = 0; i < blazorTabs.dropdownComponents.length; i++) {
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
    for (let i = 0; i < blazorTabs.dynamicTabContentComponents.length; i++) {
        let dynamicTabContentComponent = blazorTabs.dynamicTabContentComponents[i];
        let height = window.innerHeight - dynamicTabContentComponent.div.getBoundingClientRect().top;
        dynamicTabContentComponent.component.invokeMethodAsync('Resize', height);
    }
});