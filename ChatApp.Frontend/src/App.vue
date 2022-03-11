<template>
  <v-app>
    <v-navigation-drawer
      app
      mobile-breakpoint="766"
      v-model="drawer"
      :permanent="!$vuetify.breakpoint.mdAndDown"
    >
      <v-sheet color="grey lighten-4" class="pa-4">
        <v-avatar class="mb-4" color="grey darken-1" size="64"></v-avatar>

        <div>madey@articlearn.id</div>
      </v-sheet>

      <v-divider class="py-0"></v-divider>

      <v-subheader>Your contacts</v-subheader>

      <v-list class="py-0" nav dense shaped>
        <v-list-item-group v-model="group" active-class="indigo--text">
          <template v-for="(chat, index) in chats">
            <v-list-item link @click="selectedChat = chat" :to="'/dm/' + chat.name" replace :key="index">
              <v-list-item-avatar color="grey darken-1"></v-list-item-avatar>

              <v-list-item-content>
                <v-list-item-title>{{ chat.name }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </template>
        </v-list-item-group>
      </v-list>

      <template v-slot:append>
        <v-sheet color="grey lighten-4">
          <v-divider></v-divider>
          <div class="pa-2">
            <v-btn block color="red" dark>Logout</v-btn>
          </div>
        </v-sheet>
      </template>
    </v-navigation-drawer>

    <!-- <v-app-bar app flat height="72">
      <template v-if="selectedChat !== null">
        <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

        <v-app-bar-title class="text-no-wrap">{{ selectedChat.name }}</v-app-bar-title>
      </template>

      <v-spacer></v-spacer>
      <v-responsive max-width="156">
        <v-text-field dense flat hide-details rounded solo-inverted></v-text-field>
      </v-responsive>
    </v-app-bar>-->

    <router-view></router-view>
  </v-app>
</template>

<style>
.v-app-bar-title__content {
  min-width: 140px !important;
}
</style>

<script>
export default {
  data: () => ({
    chats: [
      { name: 'Contact 1' },
      { name: 'Contact 2' },
      { name: 'Contact 3' },
      { name: 'Contact 4' },
      { name: 'Contact 5' },
      { name: 'Contact 6' },
    ],
    drawer: null,
    data: null,
    group: null,
    messages: [],
    selectedChat: null,
  }),
  mounted: function () {
    this.selectedChat = this.chats[0];
  },
  methods: {
    toggleDrawer() {
      this.drawer = !this.drawer
    }
  }
};
</script>
