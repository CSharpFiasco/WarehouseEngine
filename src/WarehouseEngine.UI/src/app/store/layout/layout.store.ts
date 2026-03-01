import { signalStore, withMethods, withState, patchState } from '@ngrx/signals';

type LayoutState = {
  readonly isSideNavOpen: boolean;
};

const initialState: LayoutState = {
  isSideNavOpen: false,
};

export const LayoutStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withMethods((store) => ({
    toggleSideNav(): void {
      patchState(store, { isSideNavOpen: !store.isSideNavOpen() });
    },
    openSideNav(): void {
      patchState(store, { isSideNavOpen: true });
    },
    closeSideNav(): void {
      patchState(store, { isSideNavOpen: false });
    },
  }))
);
