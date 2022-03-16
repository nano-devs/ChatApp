<template>
  <v-main>
    <v-app-bar app flat height="72" v-if="!$vuetify.breakpoint.smAndDown">
      <v-app-bar-nav-icon @click="$router.replace('/dm/')">
        <v-icon>mdi-close</v-icon>
      </v-app-bar-nav-icon>

      <v-app-bar-title class="text-no-wrap">{{ $route.params.contactId }}</v-app-bar-title>

      <v-spacer></v-spacer>
      <v-responsive max-width="156">
        <v-text-field dense flat hide-details rounded solo-inverted></v-text-field>
      </v-responsive>
    </v-app-bar>

    <v-divider></v-divider>

    <v-footer app color="transparent" inset class="pb-6">
      <v-textarea
        background-color="grey"
        flat
        auto-grow
        solo
        rounded
        dark
        hide-details
        rows="1"
        append-icon="mdi-emoticon"
        prepend-inner-icon="mdi-plus-circle"
      ></v-textarea>
    </v-footer>
  </v-main>
</template>

<script>
export default {
  data() {
    return {
      contactId: this.$route.params.contactId,
      messages: [],
    }
  },
  created: function () {
  },
};
</script>
