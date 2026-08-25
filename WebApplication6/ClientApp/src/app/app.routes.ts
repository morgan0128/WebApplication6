import { Routes } from '@angular/router';
import { Landing } from './landing/landing';
import {EditAlbums} from './edit-albums/edit-albums';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'edit-albums', component: EditAlbums },
  { path: '**', redirectTo: '' },
];
