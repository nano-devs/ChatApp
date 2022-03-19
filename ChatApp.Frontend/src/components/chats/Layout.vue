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

        <div>
          Your ID:
          <span class="font-weight-bold">bX4Jyg</span>
        </div>
      </v-sheet>

      <v-divider class="py-0"></v-divider>

      <v-subheader>
        Your contacts
        <v-spacer></v-spacer>
        <v-btn icon>
          <v-icon>mdi-plus</v-icon>
        </v-btn>
      </v-subheader>

      <v-list class="py-0" nav dense shaped>
        <v-list-item-group v-model="group" active-class="indigo--text">
          <template v-for="(contact, index) in contacts">
            <v-list-item
              link
              @click="selectedContact = contact"
              :to="'/dm/' + contact.name"
              replace
              :key="index"
            >
              <v-list-item-avatar color="grey darken-1"></v-list-item-avatar>

              <v-list-item-content>
                <v-list-item-title>{{ contact.name }}</v-list-item-title>
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

    <v-app-bar app flat height="72" v-if="$vuetify.breakpoint.smAndDown">
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-app-bar-title class="text-no-wrap">{{ $route.params.contactId }}</v-app-bar-title>

      <v-spacer></v-spacer>
      <v-responsive max-width="156">
        <v-text-field dense flat hide-details rounded solo-inverted></v-text-field>
      </v-responsive>
    </v-app-bar>

    <!-- Render messages & footer textfield -->
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
  data: function () {
    return {
      contacts: [
        { name: 'Contact1' },
        { name: 'Contact2' },
        { name: 'Contact3' },
        { name: 'Contact4' },
        { name: 'Contact5' },
        { name: 'Contact6' },
      ],
      contactId: this.$route.params.contactId,
      drawer: null,
      data: null,
      group: null,
      messages: [],
      selectedContact: null,
    }
  },
  mounted: function () {
    this.selectedContact = this.contacts[0];
  },
  methods: {
    toggleDrawer() {
      this.drawer = !this.drawer
    }
  }
};
</script>
