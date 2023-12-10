/**
 * All credit to Angular Material team. From https://github.com/angular/material.angular.io/blob/main/src/app/shared/style-manager/style-manager.ts
 */

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StyleManagerService {
  private getClassNameForKey(key: string) {
    return `app-${key}`;
  }

  private createLinkElementWithKey(key: string) {
    const linkEl = document.createElement('link');
    linkEl.setAttribute('rel', 'stylesheet');
    linkEl.classList.add(this.getClassNameForKey(key));
    document.head.appendChild(linkEl);
    return linkEl;
  }

  public getExistingLinkElementByKey(key: string) {
    return document.head.querySelector(`link[rel="stylesheet"].${this.getClassNameForKey(key)}`);
  }

  public getLinkElementForKey(key: string) {
    return this.getExistingLinkElementByKey(key) ?? this.createLinkElementWithKey(key);
  }

  /**
   * Set the stylesheet with the specified key.
   */
  public setStyle(key: string, href: string) {
    this.getLinkElementForKey(key).setAttribute('href', href);
  }

  /**
   * Remove the stylesheet with the specified key.
   */
  public removeStyle(key: string) {
    const existingLinkElement = this.getExistingLinkElementByKey(key);
    if (existingLinkElement) {
      document.head.removeChild(existingLinkElement);
    }
  }
}
