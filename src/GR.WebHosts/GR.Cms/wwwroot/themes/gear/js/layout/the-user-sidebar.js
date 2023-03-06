Vue.component('TheUserSidebar', {
	template: `
		<div
			class="user-sidebar px-20"
			:class="{ open: isSidebarOpen }"
			v-click-outside="closeModal"
		>
			<a class="d-block mt-20px close-icon" @click.stop.prevent="closeModal"></a>
			<div class="current-date mt-20px">
				<p class="p-xs mb-0">{{ currentDate }}</p>
			</div>
			<div class="d-block mt-1 side-nav-links">
				<div class="side-nav-divider mb-2"></div>
				<a href="/account/account">       {{   t('my_account') }}</a>
				<a href="/account/editAccount">  {{   t('edit_account') }}</a>
				<a href="/taskmanager" > {{   t('my_tasks') }}</a>
				<div class="side-nav-divider my-2"></div>
				<button type="button" class="btn sa-logout">
					  {{   t('logout') }}
				</button>
			</div>
			<div class="position-relative" style="min-height: 200px">
				<div v-if="notificationsLoaded" id="notify" class="mt-4">
					<div class="d-flex">
						<h5>{{   t('notifications') }}
							<span class="badge badge-danger rounded top-webkit-inline-box d-inline notification-counter ml-1">{{ getUserNotificationsFromStore.length }}</span>
						</h5>
						<div class="d-flex flex-column ml-auto">
							<a v-if="getUserNotificationsFromStore.length > 0" class="ml-auto" href="#" @click.stop.prevent="clearAll">{{   t('clear_notifications') }}</a>
							<a v-if="haveUnreadNotifications" class="ml-auto text-danger" href="#" @click.stop.prevent="readAllUserNotifications">Read all</a>
						</div>
					</div>
					<div class="d-block side-nav-notifications w-100">
						<div class="side-nav-divider mb-1"></div>
						<div class="notification-container mt-1">
							<ul class="list-unstyled notification-list position-relative mb-0 collapsed">
								<li v-for="n in getUserNotificationsFromStore" class="notification hover-invisible-toggle" @click.stop.prevent="navigateToDetails(n.url)">
									<div class="d-flex w-100 position-relative">
										<p class="p-xs font-weight-600 color-black mb-0">{{ n.subject }}</p>
										<span class="notification-delete hover-invisible close-icon" @click.stop.prevent="deleteNotification(n.id)"></span>
										<span class="ml-auto hover-hide">
											{{ convertToRelativeTime(n.created) }}
										</span>
										<span class="read-btn" v-if="!n.isDeleted" @click.stop.prevent="readNotification(n.id)" >&nbsp;</span>
									</div>
									<div class="d-flex mt-1 w-100">
										<div class="badge badge-outline-primary user-rectangle notification-user-button">
											{{ userInitials() }}
										</div>
										<div class="d-block w-100 ml-2">
											<p class="p-sm color-black mb-0 notification-description">
												<span class="d-block">Notification</span>
												{{ n.content }}
											</p>
										</div>
									</div>
								</li >
								<hr class="my-2">
							</ul>
						</div>
					</div>
				</div>
				<div v-show="!notificationsLoaded" class="section-loader" style="background-color: #ffffff !important;"><Loader/></div>
			</div>
		</div>
	`,
	data() {
		return {
			currentDate: moment().format('dddd, MMMM D, YYYY'),
			user: {},
			notificationsLoaded: false,
			connectionUrl: window.location.origin + '/rtn',
			hubConnection: null,
			nrNotifications: 0
		}
	},
	computed: {
		isSidebarOpen() {
			return this.$store.state.userSidebarOpen;
		},
		haveUnreadNotifications() {
			if (this.getUserNotificationsFromStore.find(notification => !notification.isDeleted)) return true;
			return false;
		},
		getUserNotificationsFromStore() {
			return this.$store.state.userNotifications;
		}
	},
	created() {
		customAjaxRequest(apiEndpoints.Users.GetCurrentUserInfo).then(user => {
			this.user = user;
		});
	},
	mounted() {
		new ST().registerLocalLogout(".sa-logout");
		/*this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.connectionUrl).build();*/
		this.hubConnection = new signalR.HubConnectionBuilder()
			.withUrl(this.connectionUrl)
			.withAutomaticReconnect()
			.build();
		//{ transport: ['serverSentEvents'] }
		this.hubConnection.start().then(function () {
		}).catch(function (err) {
			return console.error(err.toString());
		});;
		this.hubConnection.on("SendClientNotification", (notifications) => {
			this.$store.dispatch('getUserNotifications', this.user.id);
		});
	},
	methods: {
		t(key) {
			return window.translate(key);
		},
		async deleteNotification(notificationId) {
			customAjaxRequest(apiEndpoints.Notifications.PermanentlyDeleteNotification, 'DELETE', { notificationId }).then(() => {
				this.$store.dispatch('getUserNotifications', this.user.id);
			});
		},
		async readNotification(notificationId) {
			customAjaxRequest(apiEndpoints.Notifications.MarkAsRead, 'POST', { notificationId }).then(() => {
				this.$store.dispatch('getUserNotifications', this.user.id);
			});
		},
		async readAllUserNotifications() {
			customAjaxRequest(apiEndpoints.Notifications.MarkAllUserNotificationsAsRead, 'POST', { userId: this.user.id }).then(() => {
				this.$store.dispatch('getUserNotifications', this.user.id);
			})
		},
		clearAll() {
			customAjaxRequest(apiEndpoints.Notifications.ClearAllByUserId, 'POST', { userId: this.user.id }).then(() => {
				this.$store.dispatch('getUserNotifications', this.user.id);
			});
		},
		closeModal() {
			this.$store.dispatch("closeUserSidebarAction");
		},
		convertToRelativeTime(date) {
			return moment(date, 'DD.MM.YYYY HH:mm:ss A').toNow(true) + ' ago';
		},
		userInitials() {
			return this.user.userLastName && this.user.userName ? `${this.user.userName.charAt(0)} ${this.user.userLastName.charAt(0)}` : 'UP';
		},
		navigateToDetails(url) {
			if (url && window.location.pathname + window.location.search !== url) { window.location.href = url }
		},
		loadUserNotifications() {
			this.$store.dispatch('getUserNotifications', this.user.id);
		}
	},
	watch: {
		isSidebarOpen(newVal) {
			if (newVal === true && !this.notificationsLoaded) {
				this.$store.dispatch('getUserNotifications', this.user.id);
			}
		},
		getUserNotificationsFromStore(newVal) {
			this.notificationsLoaded = true;
		},
		nrNotifications(newVal) {
			console.log(newVal, 'val');
			loadUserNotifications();
		}
	}
});