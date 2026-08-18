import { Routes } from '@angular/router';
import { Landing } from './landing/landing';
import { UserArt } from './user-art/user-art';
import { WorkQueue } from './work-queue/work-queue';
import {ImageLoader} from './image-loader/image-loader';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'user-art', component: UserArt },
  { path: 'work-queue', component: WorkQueue },
  { path: 'image-loader', component: ImageLoader },
  { path: '**', redirectTo: '' },
];
