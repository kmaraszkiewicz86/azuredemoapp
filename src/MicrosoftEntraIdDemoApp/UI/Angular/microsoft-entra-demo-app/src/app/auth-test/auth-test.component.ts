import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthTestServiceTsService } from './auth-test.service.ts.service';
import { ActivatedRoute } from '@angular/router';
import { lastValueFrom, Subscription } from 'rxjs';

@Component({
  selector: 'app-auth-test',
  imports: [],
  templateUrl: './auth-test.component.html',
  styleUrl: './auth-test.component.scss'
})
export class AuthTestComponent implements OnInit, OnDestroy {

  public message: string | null = null;

  private routerChangeSubscription: Subscription | null = null;

  constructor(
    private readonly authTestService: AuthTestServiceTsService,
    private readonly route: ActivatedRoute
  ) { }

  /**
   * On component initialization, we check the route parameters for a 'type' and call the appropriate API endpoint using the AuthTestService. We also handle any errors that may occur during the API call and display an appropriate message to the user.
   */
  async ngOnInit(): Promise<void> {
    this.routerChangeSubscription = this.route.params.subscribe(async () => {
      await this.loadData();
    });
  }

  ngOnDestroy(): void {
    this.routerChangeSubscription?.unsubscribe();
  }

  /**
   * Load data from api based on the 'type' route parameter. If the 'type' parameter is not valid, it defaults to 'no_available_type'. The result from the API call is then displayed to the user, and any errors are caught and displayed as well.
   */
  private async loadData() {
    const type = this.checkIfTypeExists(this.route.snapshot.paramMap.get('type') || 'empty');

    this.message = `Testing ${type} endpoint...`;
    try {
      const result = await lastValueFrom(this.authTestService.getData(type));
      this.message = result.message;
    }
    catch (error) {
      this.message = error instanceof Error ? error.message : 'An unknown error occurred.';
    }
  }

  /**
   * Checks if the provided type exists in the list of valid types.
   * @param type the type to check
   * @returns the valid type if it exists, otherwise 'no_available_type'
   */
  private checkIfTypeExists(type: string): string {
    const validTypes = ['checkGroup', 'apptesters', 'test', 'admin'];

    if (validTypes.includes(type)) {
      return type;
    } else {
      return 'no_available_type';
    }
  }
}
