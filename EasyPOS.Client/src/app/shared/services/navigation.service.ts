import { Injectable } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {

  constructor(private router: Router, private route: ActivatedRoute) { }

  /**
   * Navigate to a specific route.
   * This method can be used to navigate to any page without query or route parameters.
   * 
   * @param path - The path to navigate to (e.g., '/admin/app-menus').
   * @param queryParams - Optional query params to pass in (e.g., { userId: 123 }).
   * @param relativeToCurrent - Set to true if you want to navigate relative to the current route.
   * @param skipLocationChange - If true, navigates without updating the browser history.
   * 
   * Example:
   * this.customRouterService.navigateTo('/admin/app-menus');
   */
  navigateTo(
    path: string,
    queryParams?: any,
    relativeToCurrent: boolean = false,
    skipLocationChange: boolean = false
  ): void {
    const extras = { queryParams, skipLocationChange };
    const navigationPath = relativeToCurrent ? [path] : [path];

    this.router.navigate(navigationPath, {
      relativeTo: relativeToCurrent ? this.route : null,
      ...extras
    }).catch(error => {
      console.error(`Navigation to ${path} failed: `, error);
    });
  }

  /**
   * Navigate to a route with optional route parameters.
   * Use this method when you need to pass parameters as part of the URL.
   * 
   * @param path - The path to navigate to (e.g., '/admin/app-menus').
   * @param params - An array of route parameters (e.g., ['123'] for '/admin/app-menus/123').
   * @param relativeToCurrent - Set to true if you want to navigate relative to the current route.
   * @param skipLocationChange - If true, navigates without updating the browser history.
   */
  navigateWithParams(
    path: string,
    params: any[] = [],
    relativeToCurrent: boolean = false,
    skipLocationChange: boolean = false
  ): void {
    const extras = { skipLocationChange };
    const navigationPath = [path, ...params];

    this.router.navigate(navigationPath, {
      relativeTo: relativeToCurrent ? this.route : null,
      ...extras
    }).catch(error => {
      console.error(`Navigation to ${path} with params failed: `, error);
    });
  }

  /**
   * Navigate to a route with query parameters.
   * Use this when you need to append query parameters to a URL.
   * 
   * @param path - The path to navigate to (e.g., '/admin/app-menus').
   * @param queryParams - An object representing query parameters (e.g., { page: 2, sort: 'name' }).
   * @param relativeToCurrent - Set to true if you want to navigate relative to the current route.
   * @param skipLocationChange - If true, navigates without updating the browser history.
   */
  navigateWithQueryParams(
    path: string,
    queryParams: { [key: string]: any } = {},
    relativeToCurrent: boolean = false,
    skipLocationChange: boolean = false
  ): void {
    this.navigateTo(path, queryParams, relativeToCurrent, skipLocationChange);
  }

  /**
   * Navigate back to the previous route or to a fallback if no history exists.
   * 
   * @param fallbackPath - The fallback route in case there is no previous route (e.g., '/home').
   */
  navigateBack(fallbackPath?: string): void {
    if (window.history.length > 1) {
      this.router.navigate(['..'], { relativeTo: this.route }).catch(error => {
        console.error('Navigation back failed: ', error);
      });
    } else if (fallbackPath) {
      this.router.navigate([fallbackPath]).catch(error => {
        console.error(`Fallback navigation to ${fallbackPath} failed: `, error);
      });
    }
  }

  /**
 * Navigate to a route with optional state data.
 * Use this method when you want to pass data that is not visible in the URL.
 * 
 * @param path - The path to navigate to (e.g., '/admin/app-menus').
 * @param state - An object containing the state to pass (e.g., { success: true, message: 'Item created successfully' }).
 * @param skipLocationChange - If true, navigates without updating the browser history.
 * 
 * Example:
 * this.navigationService.navigateWithState('/admin/list-page', { success: true }, true);
 */
  navigateWithState(
    path: string,
    state: any = {},
    skipLocationChange: boolean = false
  ): void {
    const extras = { state, skipLocationChange };

    this.router.navigate([path], extras).catch(error => {
      console.error(`Navigation to ${path} with state failed: `, error);
    });
  }


  /**
   * Replace the current route with a new route.
   * This method does not keep the current route in the browser's history.
   * 
   * @param path - The path to navigate to (e.g., '/admin/app-menus').
   */
  replaceRoute(path: string): void {
    this.router.navigate([path], { replaceUrl: true }).catch(error => {
      console.error(`Route replacement to ${path} failed: `, error);
    });
  }

  /**
   * Navigate to an external URL.
   * This can be used to redirect the user to a completely external website.
   * 
   * @param url - The external URL to navigate to (e.g., 'https://google.com').
   */
  navigateToExternal(url: string): void {
    window.location.href = url;
  }

  /**
   * Reload the current route.
   * This method reloads the current page without appending it to the browser's history.
   */
  reloadCurrentRoute(): void {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    }).catch(error => {
      console.error('Route reload failed: ', error);
    });
  }

  /**
   * Check if the current URL matches a certain path.
   * Use this method to verify if the current path is the same as the given one.
   * 
   * @param path - The path to check against (e.g., '/admin/app-menus').
   * @returns boolean - True if the current route matches the path, false otherwise.
   */
  isCurrentPath(path: string): boolean {
    return this.router.url === path;
  }
}
