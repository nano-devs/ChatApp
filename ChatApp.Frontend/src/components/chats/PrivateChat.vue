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

    <!-- Render messages -->
    <v-list>
      <template v-for="(message, index) in messages">
        <v-list-item :key="index" class="message-card">
          <v-list-item-avatar style="align-self: start!important" class="mt-3">
            <v-img :src="message.avatar"></v-img>
          </v-list-item-avatar>

          <v-list-item-content>
            <v-list-item-title class="font-weight-medium">
              {{ message.author_name }}
              <span
                class="font-weight-light text-caption"
              >• {{ timeAgo(message.datetime) }}</span>
            </v-list-item-title>

            <v-card outlined class="rounded-xl rounded-tl-0">
              <v-card-text>
                <div class="text-body-2" v-html="message.content"></div>
              </v-card-text>
            </v-card>
          </v-list-item-content>
          <!-- <v-list-item-content>
            <v-list-item-title class="font-weight-medium">{{message.author_name}} <span class="font-weight-light text-caption"> • {{timeAgo(message.datetime)}}</span></v-list-item-title>
            <div class="text-body-2" v-html="message.content"></div>
          </v-list-item-content>-->
        </v-list-item>
      </template>
    </v-list>

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

<style>
.message-card:hover {
  background-color: whitesmoke;
}
</style>

<script>
export default {
  data() {
    return {
      contactId: this.$route.params.contactId,
      messages: [
        {
          content: `Lorem Ipsum is simply dummy text of the printing and typesetting industry.`,
          datetime: new Date(),
          avatar: 'https://cdn.discordapp.com/avatars/213866895806300161/77636e3c466ab3a1cdf8662f5baac5dc.png',
          author_name: 'Made Y',
        },
        {
          content: `Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.`,
          datetime: new Date(),
          avatar: 'https://cdn.discordapp.com/avatars/536892183404478483/deac7ff771720febedc2936e4479d934.png',
          author_name: 'Nano',
        },
      ],
    }
  },
  created: function () {
  },
  methods: {
    timeAgo(dateParam) {
      if (!dateParam) {
        return null;
      }
      const date = typeof dateParam === 'object' ? dateParam : new Date(dateParam);
      const DAY_IN_MS = 86400000; // 24 * 60 * 60 * 1000
      const today = new Date();
      const seconds = Math.round((today - date) / 1000);
      if (seconds < 20) {
        return 'just now';
      }
      else if (seconds < 60) {
        return 'about a minute ago';
      }
      const minutes = Math.round(seconds / 60);
      if (minutes < 60) {
        return `${minutes} minutes ago`;
      }
      const isToday = today.toDateString() === date.toDateString();
      if (isToday) {
        return 'Today'
      }
      const yesterday = new Date(today - DAY_IN_MS);
      const isYesterday = yesterday.toDateString() === date.toDateString();
      if (isYesterday) {
        return 'Yesterday'
      }
      const daysDiff = Math.round((today - date) / (1000 * 60 * 60 * 24));
      if (daysDiff < 30) {
        return `${daysDiff} days ago`;
      }
      const monthsDiff = today.getMonth() - date.getMonth() + (12 * (today.getFullYear() - date.getFullYear()));
      if (monthsDiff < 12) {
        return `${monthsDiff} months ago`;
      }
      return dateParam;
    },
  }
};
</script>
