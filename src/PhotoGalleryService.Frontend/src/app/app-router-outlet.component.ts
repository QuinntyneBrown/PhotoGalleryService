import { RouterOutlet, RouteReloadMiddleware } from "./router";
import { AuthorizedRouteMiddleware } from "./users";

export class AppRouterOutletComponent extends RouterOutlet {
    constructor(el: any) {
        super(el);
    }

    connectedCallback() {
        this.setRoutes([
            { path: "/", name: "error", authRequired: true },

            { path: "/register", name: "register" },

            { path: "/change-password", name: "change-password", authRequired: true },
            { path: "/confirm-registration/:token", name: "confirm-registration" },
            { path: "/login", name: "login" },
            { path: "/error", name: "error" },
            { path: "*", name: "not-found" }

        ] as any);

        this.use(new AuthorizedRouteMiddleware());
        this.use(new RouteReloadMiddleware());

        super.connectedCallback();
    }
}

customElements.define(`ce-app-router-oulet`, AppRouterOutletComponent);