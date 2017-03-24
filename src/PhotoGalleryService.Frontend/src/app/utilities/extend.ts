import { Constructor } from "./constructor";

export function extend<T, U, ST, SU>(mixin1: Constructor<T> & ST, mixin2: Constructor<U> & SU): Constructor<T & U> & ST & SU {
    const mixed = class { };
    Object.assign(mixed, mixin1, mixin2);
    Object.assign(mixed.prototype, mixin1.prototype, mixin2.prototype);
    return mixed as Constructor<T & U> & ST & SU;
}