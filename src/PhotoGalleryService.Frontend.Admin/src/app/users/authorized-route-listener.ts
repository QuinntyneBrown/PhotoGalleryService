import { RouterMiddleware, Router } from "../router";
import { createElement, Storage, TOKEN_KEY, Log } from "../utilities";
import { environment } from "../environment";
import { LoginRedirect } from "./login-redirect";
import { UserService } from "./user.service";
import { CurrentUser } from "./current-user";
import { User } from "./user.model";

export class AuthorizedRouteMiddleware extends RouterMiddleware {
    constructor(private _routeName = null,
        private _router: Router = Router.Instance,
        private _loginRedirect: LoginRedirect = LoginRedirect.Instance,
        private _storage: Storage = Storage.Instance,
        private _userService: UserService = UserService.Instance,
        private _currentUser: CurrentUser = CurrentUser.Instance
    ) {
        super();

    }

    public async beforeViewTransition(options: RouteChangeOptions) {     
        
        if (options.nextRoute.authRequired && !this._storage.get({ name: TOKEN_KEY })) {                      
            this._loginRedirect.setLastPath(window.location.pathname);
            options.cancelled = true;
            this._router.navigate(["login"]);                        
        }

        if (options.nextRoute.authRequired) {
            const results = await this._userService.getCurrentUser() as any;            
            if (results == "") {
                this._storage.put({ name: TOKEN_KEY, value: null });
                this._router.navigate(["login"]);
            } else {
                const user: User = results;
                this._currentUser.username = user.name;
                this._currentUser.userId = user.id;                
            }
        }

    }

    public onViewTransition(options: RouteChangeOptions): HTMLElement {
        return null;
    }

    public afterViewTransition(options: RouteChangeOptions) {

    }

}