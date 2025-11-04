import {Routes} from '@angular/router';
import {ArticlesComponent} from './articles/articles';
import {App} from './app';
import {ArticleDetailsComponent} from './article-details/article-details.component';
import {PageNotFoundComponent} from './page-not-found/page-not-found/page-not-found.component';
import {LoginDialogComponent} from './login-dialog-component/login-dialog.component';
import {HomeComponent} from './home/home.component.ts/home.component';
import {AdminPanelComponent} from './admin-panel/admin-panel/admin-panel.component';
import {adminAuthGuard} from '../guards/admin-auth-guard';
import {userLoggedInGuard} from '../guards/user-logged-in-guard';

export const routes: Routes = [

  {path: '', component: HomeComponent},
  {path: 'admin-panel', component: AdminPanelComponent, canActivate: [adminAuthGuard]},
  // {path: 'articles/:id', component: ArticleDetailsComponent},
  {path: 'home', component: HomeComponent, pathMatch: 'full'},
  {
    path: 'articles', component: ArticlesComponent, canActivate: [userLoggedInGuard],
    canActivateChild: [userLoggedInGuard], children: [
      {path: ':id', component: ArticleDetailsComponent}, //:id is a wildcard,
      // will be adapted to ui later with angular-material
    ]
  },
  {path: 'login', component: LoginDialogComponent},
  {path: '**', component: PageNotFoundComponent},
];
