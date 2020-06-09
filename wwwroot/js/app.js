window.blazorTabs = {

    dropdownComponents: [],

    registerDropdownComponent: (button, dropdownMenu, component, componentGuid) => {
        let index = blazorTabs.findDropdownComponentIndex(componentGuid);
        if (index === -1) {
            blazorTabs.dropdownComponents.push({ button: button, dropdownMenu: dropdownMenu, component: component, componentGuid: componentGuid });
        }
    },

    unregisterDropdownComponent: (componentGuid) => {
        let index = blazorTabs.findDropdownComponentIndex(componentGuid);
        if (index !== -1) {
            blazorTabs.dropdownComponents.splice(index, 1);
        }
    },

    findDropdownComponentIndex: (componentGuid) => {
        let index = -1;
        for (let i = 0; i < blazorTabs.dropdownComponents.length; i++) {
            if (blazorTabs.dropdownComponents[i].componentGuid === componentGuid) {
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
