import { User, Role, UpdateRoleRequest } from './../../../core/models/user.model';
import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { UserService } from '../../../core/services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { SaveDialogComponent } from '../../../shared/components/save-dialog/save-dialog.component';
import { StateViewComponent, ViewState } from '../../../shared/components/state-view/state-view.component';

@Component({
  selector: 'app-update-role',
  standalone: true,
  imports: [StateViewComponent],
  templateUrl: './update-role.component.html',
  styleUrl: './update-role.component.css',
})
export class UpdateRoleComponent implements OnInit {
  private dialog = inject(MatDialog);
  private userServices = inject(UserService);

  users = signal<User[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  viewState = computed<ViewState>(() => {
    if (this.loading()) return 'loading';
    if (this.error()) return 'error';
    if (this.users().length === 0) return 'empty';
    return 'success';
  });

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.loading.set(true);
    this.error.set(null);

    this.userServices.getAllUsersExceptAdmin().subscribe({
      next: (res) => {
        this.users.set(res);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load users. Please try again.');
        this.loading.set(false);
      },
    });
  }

  updateRole(user: User) {
    const dialogRef = this.dialog.open(SaveDialogComponent);

    dialogRef.afterClosed().subscribe((result: Role | undefined) => {
      if (!result) return;
      const userId = user.id;
      const req: UpdateRoleRequest = { role: result };
      this.userServices.updateRole(userId, req).subscribe({
        next: () => {
          this.users.update((list) =>
            list.map((u) => (u.id === userId ? { ...u, role: result } : u)),
          );
        },
        error: () => {
          console.error('Failed to update role');
        },
      });
    });
  }
}