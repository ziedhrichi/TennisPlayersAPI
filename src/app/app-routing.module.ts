import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlayersListComponent } from './players-list/players-list.component';
import { PlayerDetailComponent } from './player-detail/player-detail.component';
import { AddEditPlayerComponent } from './add-edit-player/add-edit-player.component';

const routes: Routes = [
  { path: 'players', component: PlayersListComponent },
  { path: 'player/:id', component: PlayerDetailComponent },
  { path: 'add-player', component: AddEditPlayerComponent },
  { path: 'edit-player/:id', component: AddEditPlayerComponent },
  { path: '', redirectTo: '/players', pathMatch: 'full' },
  { path: '**', redirectTo: '/players' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}