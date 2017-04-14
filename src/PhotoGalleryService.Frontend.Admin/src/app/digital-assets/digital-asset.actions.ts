import { DigitalAsset } from "./digital-asset.model";

export const digitalAssetActions = {
    ADD: "[DigitalAsset] Add",
    EDIT: "[DigitalAsset] Edit",
    DELETE: "[DigitalAsset] Delete",
};

export class DigitalAssetEvent extends CustomEvent {
    constructor(eventName:string, digitalAsset: DigitalAsset) {
        super(eventName, {
            bubbles: true,
            cancelable: true,
            detail: { digitalAsset }
        });
    }
}

export class DigitalAssetAdd extends DigitalAssetEvent {
    constructor(digitalAsset: DigitalAsset) {
        super(digitalAssetActions.ADD, digitalAsset);        
    }
}

export class DigitalAssetEdit extends DigitalAssetEvent {
    constructor(digitalAsset: DigitalAsset) {
        super(digitalAssetActions.EDIT, digitalAsset);
    }
}

export class DigitalAssetDelete extends DigitalAssetEvent {
    constructor(digitalAsset: DigitalAsset) {
        super(digitalAssetActions.DELETE, digitalAsset);
    }
}
